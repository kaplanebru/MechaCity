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

    [SerializeField] private bool newChainData;
    private string chainDataName;


    [SerializeField] private CogData cogData;


    public Machinery machineryPrefab;
    private GUIStyle _narrowButton;

    [SerializeField] int cogToDestroyIndex;


    private void OnEnable()
    {
    }
    
    public override void OnInspectorGUI()
    {
        if (EditorApplication.isPlaying) return;
        DrawDefaultInspector();

        if (machineryPrefab == null)
            machineryPrefab = target as Machinery;


        GUILayout.Label("Chain & Cog Generator", EditorStyles.boldLabel);
        _narrowButton = new GUIStyle(GUI.skin.button);
        _narrowButton.fixedWidth = 200f;

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("SAVE CHANGES"))
        {
            SaveMachinery();
        }

        if (GUILayout.Button("SAVE ONTO EXISTING PREFAB"))
        {
            machineryPrefab.SaveOnExistingPrefab();
            SaveMachinery();
        }
        EditorGUILayout.EndHorizontal();

        if (cogs == null || (cogs.Length > 0 && cogs[0] == null))
            cogs = machineryPrefab.cogHolder.RestoreCogsInEditor();

        if (cogHolderLabels == null || cogHolderLabels.Length != cogs.Length)
        {
            cogHolderLabels = new string[cogs.Length];
            cogHolderLabels = cogs.Select(x => x.ToString()).ToArray();
        }

        if (GUILayout.Button("Reset To 2D Space"))
            machineryPrefab.To2D();

        EditorGUILayout.Space();
        
        GUILayout.Label("COG SETTINGS", EditorStyles.boldLabel); //\n 
        //MyEditorHelpers.DrawSeparatorLine(Color.gray);

        ShowGizmosOnSelection();
        EditorGUILayout.Space();

        Color originalBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.yellow;
        selectedIndex = EditorGUILayout.Popup("Selected Cog :", selectedIndex, cogHolderLabels);
        GUI.backgroundColor = originalBackgroundColor;

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.Space();


        if (selectedIndex >= 0 && selectedIndex < cogs.Length)
        {
            EditorGUI.indentLevel++;
            FillCogData(selectedIndex);
            machineryPrefab.cogHolder.DrawGizmosOnSelectedCog(selectedIndex);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
        MyEditorHelpers.DrawFrames(Color.gray, GUILayoutUtility.GetLastRect());

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Cogs"))
            SaveCogs();
        
        EditorGUILayout.Space();


        GUILayout.Label(" _______________ADD or REMOVE COG_______________ ", EditorStyles.centeredGreyMiniLabel); //\n 
        EditorGUILayout.Space();
        machineryPrefab.cogHolder.cogPrefab = (Cogwheel) EditorGUILayout.ObjectField("Cog Prefab",  machineryPrefab.cogHolder.cogPrefab, typeof(Cogwheel), false);
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
        cogToDestroyIndex = EditorGUILayout.Popup("Remove Selected Cog", cogToDestroyIndex, cogHolderLabels);
        if (GUILayout.Button("Remove"))
            RemoveCog();
        GUILayout.EndHorizontal();


        EditorGUILayout.Space();

        ////////////////CHAIN RELATED///////////////////////////////
        machineryPrefab.isChainRelated = EditorGUILayout.Toggle("Chain Related", machineryPrefab.isChainRelated);

        if (machineryPrefab.isChainRelated)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("CHAIN PROPERTIES", EditorStyles.boldLabel);
            MyEditorHelpers.DrawSeparatorLine(Color.gray);
            EditorGUILayout.Space();
            
            newChainData = EditorGUILayout.Toggle("Create New Chain Data", newChainData);
            
            if (newChainData)
            {
                chainDataName = EditorGUILayout.TextField("Chain Data Name", chainDataName); //write the same name if you want to modify pool + reset pool before

                if (GUILayout.Button("Apply")) 
                {
                    CreateChainData();
                    newChainData = false;
                }
            }

            machineryPrefab.ChainData = (ChainData) EditorGUILayout.ObjectField("Use Selected Chain Data",  machineryPrefab.ChainData , typeof(ChainData),
                false);


            if (machineryPrefab.ChainData  != null)
            {
                FillChainData();

                if (GUILayout.Button("Generate Chain"))
                    GenerateChain();
                
                if (GUILayout.Button("Delete Link Pool"))
                    DeleteLinkPool();

                if (GUILayout.Button("Deactivate Link pool"))
                    machineryPrefab.ResetLinks();
            }
        }

        EditorGUILayout.Space();
        EditorGUI.EndChangeCheck();
    }

    void ShowGizmosOnSelection()
    {
        EditorGUI.BeginChangeCheck();
        machineryPrefab.cogHolder.showGizmos = EditorGUILayout.Toggle("Show Gizmos On Selected Cog", machineryPrefab.cogHolder.showGizmos);
        if (EditorGUI.EndChangeCheck())
        {
            if (!machineryPrefab.cogHolder.showGizmos)
                machineryPrefab.cogHolder.DisableAllGizmos();
            else
                machineryPrefab.cogHolder.DrawGizmosOnSelectedCog(selectedIndex);
            Repaint();
        }
    }


    void AddCog(bool isNew)
    {
        machineryPrefab.cogHolder.AddCog(isNew, cogData);
        cogs = machineryPrefab.cogHolder.RestoreCogsInEditor();
        selectedIndex = cogs.Length - 1;
        cogToDestroyIndex = selectedIndex;
        GenerateChain();
        Repaint();
    }

    void RemoveCog()
    {
        machineryPrefab.cogHolder.RemoveCog(cogToDestroyIndex);
        cogs = machineryPrefab.cogHolder.RestoreCogsInEditor();
        
        if (cogs.Length > 0)
        {
            if (selectedIndex == cogToDestroyIndex)
            {
                int newIndex;
                do
                {
                    newIndex = UnityEngine.Random.Range(0, cogs.Length);
                } while (newIndex == selectedIndex);

                selectedIndex = newIndex;
            }
            cogToDestroyIndex = cogs.Length - 1;
        }
        GenerateChain();
        Repaint();
    }

   

  

    void CreateChainData()
    {
        machineryPrefab.ChainData = CreateInstance<ChainData>();

        AssetDatabase.CreateAsset(machineryPrefab.ChainData,
            MyEditorHelpers.WriteAssetPath(chainDataName, "ChainDatas")); //(chainData, "Assets/chainData.asset"); 
        Debug.Log(MyEditorHelpers.WriteAssetPath(chainDataName, "ChainDatas"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    void SaveMachinery() //todo: buralar machinerye taşınabilir
    {
        Debug.Log("saved");

        EditorUtility.SetDirty(machineryPrefab.ChainData); //TODO: sonradan eklendi
        EditorUtility.SetDirty(target);

        if (MyPrefabHelpers.IsPrefabInstance(machineryPrefab.gameObject))
            MyPrefabHelpers.OverrideChanges(machineryPrefab.gameObject);
        else
            machineryPrefab.residual.CleanResiduals();
        
    }
    
    void GenerateChain()
    {
        machineryPrefab.GenerateChain(SaveCogs);
    }
    
    void SaveCogs()
    {
        foreach (var cog in cogs)
        {
            EditorUtility.SetDirty(cog.Data);
        }
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
            otherCogData = (CogData) EditorGUILayout.ObjectField("Cog Data", otherCogData, typeof(CogData), false);
            if (GUILayout.Button("Apply"))
            {
                if (otherCogData == null) return;

                cogData = otherCogData;
                machineryPrefab.cogHolder.cogs[i].Data = otherCogData;
                GenerateChain();
                changeCogData = false;
                //Repaint();
                // SaveMachinery();
            }
        }

        if (changeWithNewCogData)
        {
            if (GUILayout.Button("Apply"))
            {
                cogs[i].Data = machineryPrefab.cogHolder.CreateCogData("Cog " + machineryPrefab.cogHolder.newCogIndex++);
                changeWithNewCogData = false;
                GenerateChain();
                Repaint();
                SaveMachinery();
            }
        }
    }

    void FillCogData(int i)
    {
        CogData Data = cogs[i].Data;

        if (Data == null)
        {
            Debug.Log(cogs.Length);
            Debug.Log(cogs[i].name);

            return;
        }

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Data Name");
        EditorGUILayout.LabelField(Data.name, EditorStyles.helpBox);
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        Data.Radius = EditorGUILayout.FloatField("Radius", Data.Radius);
        if (EditorGUI.EndChangeCheck())
        {
            SetupCogsWithSameData(i);
            GenerateChain();
        }

        EditorGUI.BeginChangeCheck();
        Data.ContactType = (ChainEnums.CogContactType) EditorGUILayout.EnumPopup("Contact Type", Data.ContactType);
        if (Data.ContactType == ChainEnums.CogContactType.CogRelated)
        {
            Data.RelatedCog =
                (Cogwheel) EditorGUILayout.ObjectField("Related Cog", Data.RelatedCog, typeof(Cogwheel), true);
        }

        Data.HoleSize = EditorGUILayout.FloatField("Hole Size", Data.HoleSize);
        Data.HoleType = (ChainEnums.HoleType) EditorGUILayout.EnumPopup("Hole Type", Data.HoleType);

        if (!machineryPrefab.isChainRelated || Data.ContactType == ChainEnums.CogContactType.Indifferent)
        {
            Data.IsMoving = EditorGUILayout.Toggle("Is Moving", Data.IsMoving);
            Data.RotationDirection = EditorGUILayout.IntField("Rotation Direction", Data.RotationDirection);
        }

        if (EditorGUI.EndChangeCheck())
            AccidentalSetupCogsWithSameData(i);


        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Teeth Settings", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();
        GUILayout.BeginHorizontal();
        Data.TeethPoolPrefab = (TeethPool) EditorGUILayout.ObjectField("Teeth Pool Prefab",
            Data.TeethPoolPrefab, typeof(TeethPool), false);
        if (GUILayout.Button("Apply")) //, narrowButton))
            HandleTeethPoolChange(i);
        GUILayout.EndHorizontal();

        Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
        Data.ToothGap = EditorGUILayout.FloatField("Tooth Gap", Data.ToothGap);
        Data.Equalize = EditorGUILayout.Toggle("Equal Gaps", Data.Equalize);
        Data.MinGapLimit = EditorGUILayout.FloatField("Min Gap Limit", Data.MinGapLimit);

        EditorGUILayout.Space();
        ChangeCogData(i);

        if (EditorGUI.EndChangeCheck())
            SetupCogsWithSameData(i);
        //EditorUtility.SetDirty(cogs[i].Data); //TODO: removed
    }

    void SetupCogsWithSameData(int i)
    {
        Cogwheel selectedCog = cogs[i];
        foreach (var cog in cogs)
        {
            if (cog.Data == selectedCog.Data)
                cog.Setup();
        }
    }

    void AccidentalSetupCogsWithSameData(int i)
    {
        Cogwheel selectedCog = cogs[i];
        foreach (var cog in cogs)
        {
            if (cog.Data == selectedCog.Data)
                cog.AccidentalSetup();
        }
    }

    //private ChainLink lastLinkPrefab;

    void FillChainData()
    {
        ChainData chainData = machineryPrefab.ChainData;
        chainData.OnTesting = EditorGUILayout.Toggle("On Testing", chainData.OnTesting);
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
            HandleLinksPoolChange();

        GUILayout.EndHorizontal();

        chainData.SetRadiusByGear = EditorGUILayout.Toggle("Set Radius By Cog", chainData.SetRadiusByGear);
        chainData.IsMoving = EditorGUILayout.Toggle("Is Moving", chainData.IsMoving);

        if (chainData.IsMoving)
        {
            //chainData.MachinerySpeed = EditorGUILayout.FloatField("Machinery Speed", chainData.MachinerySpeed);
            machineryPrefab.machinerySpeed = EditorGUILayout.FloatField("Machinery Speed", machineryPrefab.machinerySpeed);
            chainData.SpeedMultiplier = EditorGUILayout.FloatField("Speed Multiplier", chainData.SpeedMultiplier);
            chainData.LinkRotationMultiplier =
                EditorGUILayout.FloatField("Link Rotation Multiplier", chainData.LinkRotationMultiplier);

            chainData.motionDirection =
                (ChainEnums.ChainDirection) EditorGUILayout.EnumPopup("Motion Direction", chainData.motionDirection);
            chainData.SetMotionByGear = EditorGUILayout.Toggle("Set Motion By Cog", chainData.SetMotionByGear);
            chainData.LinkRotationEffect = EditorGUILayout.Toggle("Rotate Links", chainData.LinkRotationEffect);
        }
    }

    void HandleLinksPoolChange()
    {
        DeleteLinkPool();
        GenerateChain();
    }

    void HandleTeethPoolChange(int i)
    {
        DeleteTeethPool(i);
        SaveCogs();
    }

    void DeleteLinkPool()
    {
        if (MyPrefabHelpers.IsPrefabInstance(machineryPrefab.gameObject))
        {
            Debug.LogWarning("Change pool from prefab view");
            return;
        }

        machineryPrefab.DeletePoolClearLinks();
        SaveMachinery();
        machineryPrefab.CreatePool();
    }

    void DeleteTeethPool(int i)
    {
        if (MyPrefabHelpers.IsPrefabInstance(machineryPrefab.gameObject))
        {
            Debug.LogWarning("Change pool from prefab view");
            return;
        }

        ChainEvents.OnDeleteTeethPool?.Invoke(cogs[i].Id);
        SaveMachinery();
        ChainEvents.OnCreateTeethPool?.Invoke(cogs[i].Id);
    }


    private void OnDisable()
    {
    }
}