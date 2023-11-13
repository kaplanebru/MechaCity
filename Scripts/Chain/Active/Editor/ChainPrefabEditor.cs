using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Chain;
using PlasticPipe.PlasticProtocol.Client;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Machinery))]
public class ChainPrefabEditor : Editor
{
    [SerializeField] private Cogwheel[] cogs;
    private string[] cogHolderLabels;
    private int selectedIndex = 0;

    private bool isChainRelated = true;
    [SerializeField] private bool newChainData;
    private string chainDataName;


    [SerializeField] private ChainData chainData;
    [SerializeField] private CogData cogData;
    private LinksPool _linksPool;


    public Machinery machineryPrefab;

    private GUIStyle narrowButton;

    [SerializeField] int cogToDestroy;

 

    private void OnEnable()
    {
        ChainEvents.OnLinksReady += SaveMachinery;
    }



    public override void OnInspectorGUI()
    {
        if (EditorApplication.isPlaying) return;
        DrawDefaultInspector(); 

        if (machineryPrefab == null) 
            machineryPrefab = target as Machinery;


        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);
        narrowButton = new GUIStyle(GUI.skin.button);
        narrowButton.fixedWidth = 200f;

        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("SAVE CHANGES"))
        {
            SaveMachinery();
        }
        
        if (GUILayout.Button("SAVE ONTO EXISTING PREFAB"))
        {
            SaveOnExistingPrefab();
            SaveMachinery();
        }

        if (cogs == null || (cogs.Length > 0 && cogs[0] == null))
            cogs = machineryPrefab.cogHolder.GetRestoredCogs();

        if (cogHolderLabels == null || cogHolderLabels.Length != cogs.Length)
        {
            cogHolderLabels = new string[cogs.Length];
            cogHolderLabels = cogs.Select(x => x.ToString()).ToArray();
        }

        SetMachinaryChainRelation();
        if(GUILayout.Button("Reset To 2D Space"))
            machineryPrefab.To2D();
        

        EditorGUILayout.Space();

        if (cogs.Length > 0)
        {
            GUILayout.Label(" _______________COG SETTINGS_______________ ", EditorStyles.centeredGreyMiniLabel); //\n 
            EditorGUILayout.Space();

            selectedIndex = EditorGUILayout.Popup("Cog To Set", selectedIndex, cogHolderLabels);

            if (selectedIndex >= 0 && selectedIndex < cogs.Length)
            {
                EditorGUI.indentLevel++;
                SetCogData(selectedIndex);
                EditorGUI.indentLevel--;
                
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Generate Cog"))
                GenerateCogs();

            if (GUILayout.Button("Delete Teeth"))
            {
                foreach (var cog in cogs)
                {
                    var teeth = cog.GetComponent<TeethGenerator>(); //todo: event?
                    teeth.DeleteTeeth();
                }
            }
        }
        
        
        EditorGUILayout.Space();
        
        GUILayout.Label(" _______________ADD or REMOVE COG_______________ ", EditorStyles.centeredGreyMiniLabel); //\n 
        EditorGUILayout.Space();
        
        GUILayout.BeginHorizontal();
        //GUILayout.FlexibleSpace();
        EditorGUILayout.LabelField("Add Cog With Creating New Data");
        if (GUILayout.Button("Apply"))
            AddCog(true);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        cogData = (CogData) EditorGUILayout.ObjectField("Add Cog With Selected Data", cogData, typeof(CogData), false);
        if (GUILayout.Button("Apply"))
            AddCog(false);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        cogToDestroy = EditorGUILayout.Popup("Remove Selected Cog", cogToDestroy, cogHolderLabels);
        if (GUILayout.Button("Remove"))
            RemoveCog();
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();
        
        ////////////////CHAIN RELATED///////////////////////////////

        if (machineryPrefab.isChainRelated)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("_______________Chain Properties_______________", EditorStyles.boldLabel);
            EditorGUILayout.Space();


            if (chainData == null) //Get old data
            {
                if (machineryPrefab.chainSpawner.Data != null)
                    chainData = machineryPrefab.chainSpawner.Data;
            }

            newChainData = EditorGUILayout.Toggle("Create New Chain Data", newChainData);
            

            if (newChainData)
            {
                chainDataName =
                    EditorGUILayout.TextField("Chain Data Name",
                        chainDataName); //write the same name if you want to modify pool + reset pool before

                if (GUILayout.Button("Apply"))
                {
                    CreateChainData();
                    newChainData = false;
                }
            }
            
            chainData = (ChainData) EditorGUILayout.ObjectField("Use Selected Chain Data", chainData, typeof(ChainData), false);


            if (chainData != null)
            {
                SetChainData();
                GetLinkPool();

                if (GUILayout.Button("Generate Chain"))
                    GenerateChain();


                if (GUILayout.Button("Delete Link Pool"))
                    DeleteLinks();

                if (GUILayout.Button("Deactivate Link pool"))
                {
                    machineryPrefab.chainDrawer.ResetLinks();
                    //machineryPrefab.chainDrawer._chainPoints.Clear();
                }
            }
        }

        EditorGUILayout.Space();
        EditorGUI.EndChangeCheck();
    }


    void AddCog(bool isNew)
    {
        var newCog = Instantiate(machineryPrefab.assetHolder.CogPrefab, machineryPrefab.cogHolder.transform);
        newCog.name = "Cog " + machineryPrefab.cogHolder.newCogIndex++;
        newCog.AddData(isNew ? CreateCogData(newCog.name) : cogData);

        cogs = machineryPrefab.cogHolder.AddCog(newCog);
        
        Repaint();
    }

    void RemoveCog()
    {
        //DestroyImmediate(cogs[cogIndex].gameObject);
        if (cogs.Length == 0 || cogs == null) return;
        var cogsToDestroy = cogs[cogToDestroy];
        
        ChainEvents.OnDeleteObject?.Invoke(cogsToDestroy.transform);

        cogsToDestroy.gameObject.SetActive(false);
        cogs = machineryPrefab.cogHolder.RemoveCog(cogsToDestroy);
        machineryPrefab.chainDrawer.ResetLinks(); //todo: is it necessary?
    }


    void GetLinkPool()
    {
        if (_linksPool == null)
        {
            _linksPool = machineryPrefab.GetComponentInChildren<LinksPool>();
            if (_linksPool == null)
            {
                _linksPool = Instantiate(chainData.LinksPoolPrefab); //, machineryPrefab.transform);
                _linksPool.transform.SetParent(machineryPrefab.transform);
            }

            machineryPrefab.chainDrawer.GetLinksPool(_linksPool);
            
            //SaveMachinery();
        }
    }

    void SetMachinaryChainRelation()
    {
        //isChainRelated = EditorGUILayout.Toggle("Is Chain Related", isChainRelated);

        GUILayout.BeginHorizontal();
        if (GUILayout.Toggle(isChainRelated, "Is Chain Related", "Button")) //(GUILayout.Button("Is Chain Related"))
        {
            isChainRelated = true;
            machineryPrefab.isChainRelated = isChainRelated;
        }

        if (GUILayout.Toggle(!isChainRelated, "Not Chain Related", "Button")) //(GUILayout.Button("Not Chain Related"))
        {
            isChainRelated = false;
            machineryPrefab.isChainRelated = isChainRelated;
        }

        GUILayout.EndHorizontal();
    }

    CogData CreateCogData(string cogName)
    {
        var cogData = CreateInstance<CogData>();
        AssetDatabase.CreateAsset(cogData, MyEditorHelpers.WriteAssetPath(cogName + " Data", "CogDatas"));

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return cogData;
    }

    void CreateChainData()
    {
        chainData = CreateInstance<ChainData>();

        AssetDatabase.CreateAsset(chainData,
            MyEditorHelpers.WriteAssetPath(chainDataName, "ChainDatas")); //(chainData, "Assets/chainData.asset"); 
        Debug.Log(MyEditorHelpers.WriteAssetPath(chainDataName, "ChainDatas"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    void SaveMachinery()
    {
        Debug.Log("saved");
        if (isChainRelated)
            machineryPrefab.chainSpawner.Data = chainData;

        EditorUtility.SetDirty(target);
        
        if(machineryPrefab.IsPrefabInstance())
            OverrideChanges();
        else
        {
            machineryPrefab.residual.CleanResiduals(); 
        }
    }

    void OverrideChanges()
    {
        Debug.Log("ovverride");
        PrefabUtility.ApplyPrefabInstance(machineryPrefab.gameObject, InteractionMode.UserAction);
    }

    void SaveOnExistingPrefab()
    {
        Debug.Log(machineryPrefab.name);
        
        GameObject newInstance = Instantiate(machineryPrefab.gameObject);
        PrefabUtility.SaveAsPrefabAsset(newInstance,
            MyEditorHelpers.FindPathByGuid(machineryPrefab.name));
        
        DestroyImmediate(newInstance);
    }

    void GenerateChain()
    {
        // _linksPool.transform.position = Vector3.zero;
        // _linksPool.transform.rotation = Quaternion.identity;
        machineryPrefab.chainSpawner.Data = chainData;
        int chainRelatedCogAmount = 0;
        foreach (var cog in cogs)
        {
            if (cog.Data.ContactType != ChainEnums.CogContactType.ChainRelated) continue;
            chainRelatedCogAmount++;
            cog.Data.IsMoving = chainData.IsMoving;
        }

        GenerateCogs();
        
        chainData.CogAmount = chainRelatedCogAmount;
        ChainEvents.OnChainRequest?.Invoke(machineryPrefab.cogHolder.GetChainRelatedCogs(), machineryPrefab.chainSpawner); //ninvoke pas en enable
        //Repaint();
    }

    void DeleteLinks()
    {
        if (machineryPrefab.IsPrefabInstance())
        {
            Debug.LogWarning("Change pool from prefab view");
            return;
        }
        
        _linksPool.DeleteLinks();
        SaveMachinery();
        
        //EditorUtility.SetDirty(target);
    }

    void GenerateCogs()
    {
        foreach (var cog in cogs)
        {
            EditorUtility.SetDirty(cog.Data);
        }

        ChainEvents.OnCogSetupRequest.Invoke(); //parenta yollanır(machinery), ordan çocuklara gider. Parentı da çek ederiz.
    }

    private bool changeCogData = false;
    private bool changeWithNewCogData = false;

    private CogData otherCogData;
    void ChangeCogData(int i)
    {
        GUILayout.BeginHorizontal();
        changeCogData = EditorGUILayout.Toggle("Change With Other Data", changeCogData);
        changeWithNewCogData = EditorGUILayout.Toggle("Change With New Data", changeWithNewCogData);
        GUILayout.EndHorizontal();
        
        if (changeCogData)
        {
             otherCogData= (CogData) EditorGUILayout.ObjectField("Cog Data", otherCogData, typeof(CogData), false);
            if (GUILayout.Button("Apply"))
            {
                if (otherCogData == null) return;
                
                cogData = otherCogData;
                machineryPrefab.cogHolder.cogs[i].Data = otherCogData;
                changeCogData = false;
                //Repaint();
               // SaveMachinery();
            }
                
        }

        if (changeWithNewCogData)
        {
            if (GUILayout.Button("Apply"))
            {
                cogs[i].Data = CreateCogData("Cog " + machineryPrefab.cogHolder.newCogIndex++);
                changeWithNewCogData = false;
                Repaint();
                SaveMachinery();
            }
        }
    }
    void SetCogData(int i)
    {
        CogData Data = cogs[i].Data;
        
        if (Data == null)
        {
            Debug.Log(cogs.Length);
            Debug.Log(cogs[i].name);
            // cogs[i].Data = machineryPrefab.cogHolder.cogs[i].Data;
            // Data = cogs[i].Data;
            // if(Data == null)
                return;
            //     goto StartCog;
            
        }
        
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Data Name");
        EditorGUILayout.LabelField(Data.name, EditorStyles.helpBox);
        GUILayout.EndHorizontal();
        
        EditorGUILayout.Space();

        Data.Radius = EditorGUILayout.FloatField("Radius", Data.Radius);
        Data.ContactType = (ChainEnums.CogContactType) EditorGUILayout.EnumPopup("Contact Type", Data.ContactType);
        if (Data.ContactType == ChainEnums.CogContactType.CogRelated)
        {
            Data.RelatedCog =
                (Cogwheel) EditorGUILayout.ObjectField("Related Cog", Data.RelatedCog, typeof(Cogwheel), true);
        }

        Data.circularThickness = EditorGUILayout.FloatField("Thickness", Data.circularThickness);
        Data.HoleType = (ChainEnums.HoleType) EditorGUILayout.EnumPopup("Hole Type", Data.HoleType);

        if (!machineryPrefab.isChainRelated || Data.ContactType == ChainEnums.CogContactType.Indifferent)
        {
            Data.IsMoving = EditorGUILayout.Toggle("Is Moving", Data.IsMoving);
            Data.RotationDirection = EditorGUILayout.IntField("Rotation Direction", Data.RotationDirection);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Teeth Settings", EditorStyles.boldLabel);
        Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
        Data.ToothGap = EditorGUILayout.FloatField("Tooth Gap", Data.ToothGap);
        Data.Equalize = EditorGUILayout.Toggle("Equal Gaps", Data.Equalize);
        Data.MinGapLimit = EditorGUILayout.FloatField("Min Gap Limit", Data.MinGapLimit);
        
        EditorGUILayout.Space();
        ChangeCogData(i);

        //EditorUtility.SetDirty(cogs[i].Data); //TODO: removed
    }

    //private ChainLink lastLinkPrefab;

    void SetChainData()
    {
        chainData.OnTesting =  EditorGUILayout.Toggle("On Testing", chainData.OnTesting);
        chainData.Type = (ChainEnums.ChainType) EditorGUILayout.EnumPopup("Type", chainData.Type);
        chainData.Unit = EditorGUILayout.FloatField("Unit", chainData.Unit);
        chainData.RadiusOffset =
            EditorGUILayout.FloatField("Radius Offset",
                chainData.RadiusOffset); //todo: adı cog offset olarak değiştirilebilir
        chainData.Tension = EditorGUILayout.FloatField("Tension", chainData.Tension);

        GUILayout.BeginHorizontal();
        chainData.LinksPoolPrefab = (LinksPool) EditorGUILayout.ObjectField("Links Pool Prefab",
            chainData.LinksPoolPrefab, typeof(LinksPool), false);

      
        if (GUILayout.Button("Apply")) //, narrowButton))
            HandlePoolChange();
       
        GUILayout.EndHorizontal();

        chainData.SetRadiusByGear = EditorGUILayout.Toggle("Set Radius By Cog", chainData.SetRadiusByGear);
        chainData.IsMoving = EditorGUILayout.Toggle("Is Moving", chainData.IsMoving);

        if (chainData.IsMoving)
        {
            chainData.MachinerySpeed = EditorGUILayout.FloatField("Machinery Speed", chainData.MachinerySpeed);
            chainData.SpeedMultiplier = EditorGUILayout.FloatField("Speed Multiplier", chainData.SpeedMultiplier);
            chainData.LinkRotationMultiplier =
                EditorGUILayout.FloatField("Link Rotation Multiplier", chainData.LinkRotationMultiplier);

            chainData.motionDirection =
                (ChainEnums.ChainDirection) EditorGUILayout.EnumPopup("Motion Direction", chainData.motionDirection);
            chainData.SetMotionByGear = EditorGUILayout.Toggle("Set Motion By Cog", chainData.SetMotionByGear);
            chainData.LinkRotationEffect = EditorGUILayout.Toggle("Rotate Links", chainData.LinkRotationEffect);
        }
    }

    void HandlePoolChange()
    {
        DeleteLinks();
        GetLinkPool();
        GenerateChain();
    }


    private void OnDisable()
    {
        ChainEvents.OnLinksReady -= SaveMachinery;
    }
}

