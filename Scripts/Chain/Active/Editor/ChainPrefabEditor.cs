using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Chain;
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

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the default Inspector

        machineryPrefab = target as Machinery;
        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);
        narrowButton = new GUIStyle(GUI.skin.button);
        narrowButton.fixedWidth = 200f;
        
        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("SAVE CHANGES"))
            SaveMachinery();
        
        if (GUILayout.Button("OVERRIDE CHANGES ON SCENE"))
        {
            SaveMachinery();
            OverrideChanges();
        }

        if (cogs == null || (cogs.Length > 0 && cogs[0] == null))
            cogs = machineryPrefab.cogHolder.GetComponentsInChildren<Cogwheel>();
        
        if (cogHolderLabels == null || cogHolderLabels.Length != cogs.Length)
        {
            cogHolderLabels = new string[cogs.Length];
            cogHolderLabels = cogs.Select(x => x.ToString()).ToArray();
        }
        
        SetMachinaryChainRelation();

        EditorGUILayout.Space();

        GUILayout.Label(" _______________Add or Remove Cog_______________ ", EditorStyles.centeredGreyMiniLabel); //\n 
        
        GUILayout.BeginHorizontal();
        cogData = (CogData) EditorGUILayout.ObjectField("Old Cog Data", cogData, typeof(CogData), false);
        if (GUILayout.Button("Add Cog With Old Data"))
            AddCog(false);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Add Cog With New Data"))
            AddCog(true);

        GUILayout.BeginHorizontal();
        cogToDestroy = EditorGUILayout.Popup("Cog to destroy", cogToDestroy, cogHolderLabels);
        if (GUILayout.Button("Remove Cog"))
            RemoveCog();
        GUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        GUILayout.Label(" _______________Cog Settings_______________ ", EditorStyles.centeredGreyMiniLabel); //\n 

        selectedIndex = EditorGUILayout.Popup("Cog To Set", selectedIndex, cogHolderLabels);

        if (selectedIndex >= 0 && selectedIndex < cogs.Length)
        {
            EditorGUI.indentLevel++;
            SetCogData(selectedIndex);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Generate Cog"))
            GenerateCog();

        if (GUILayout.Button("Delete Teeth"))
        {
            foreach (var cog in cogs)
            {
                var teeth = cog.GetComponent<TeethGenerator>(); //todo: event?
                teeth.DeleteTeeth();
            }
        }


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
            chainData = (ChainData) EditorGUILayout.ObjectField("Chain Data", chainData, typeof(ChainData), false);

            if (newChainData)
            {
                chainDataName =
                    EditorGUILayout.TextField("Chain Data Name",
                        chainDataName); //write the same name if you want to modify pool + reset pool before

                if (GUILayout.Button("Create New Chain Data"))
                    CreateChainData();
            }


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
        
        cogs = machineryPrefab.cogHolder.GetComponentsInChildren<Cogwheel>();
        ChainEvents.OnCogsUpdated?.Invoke(cogs);
    }

    void RemoveCog()
    {
        //DestroyImmediate(cogs[cogIndex].gameObject);
        if (cogs.Length == 0 || cogs == null) return;

        cogs[cogToDestroy].gameObject.SetActive(false);
        cogs = machineryPrefab.cogHolder.GetComponentsInChildren<Cogwheel>();
        ChainEvents.OnCogsUpdated?.Invoke(cogs);

        machineryPrefab.chainDrawer.ResetLinks(); //todo: is it necessary?
    }


    void GetLinkPool()
    {
        if (_linksPool == null)
        {
            _linksPool = machineryPrefab.GetComponentInChildren<LinksPool>();
            if (_linksPool == null)
            {
                _linksPool = Instantiate(chainData.LinksPoolPrefab, target as Transform);
            }

            machineryPrefab.chainDrawer.GetLinksPool(_linksPool);
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
            MyEditorHelpers.WriteAssetPath(chainDataName, "ChainDatas")); //(chainData, "Assets/chainData.asset"); //TODO ismine +1 eklenir foldera bakılıp
        Debug.Log(MyEditorHelpers.WriteAssetPath(chainDataName, "ChainDatas"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }


    void SaveMachinery()
    {
        if (isChainRelated)
            machineryPrefab.chainSpawner.Data = chainData;

        EditorUtility.SetDirty(target);
    }

    void OverrideChanges()
    {
        PrefabUtility.ApplyPrefabInstance(machineryPrefab.gameObject, InteractionMode.UserAction);
    }

    void GenerateChain()
    {
        _linksPool.transform.position = Vector3.zero;
        _linksPool.transform.rotation = Quaternion.identity;
        machineryPrefab.GetComponentInChildren<ChainSpawner>().Data = chainData;
        foreach (var cog in cogs)
        {
            cog.Data.IsMoving = chainData.IsMoving;
        }

        GenerateCog();

        ChainEvents.OnChainRequest?.Invoke(); //ninvoke pas en enable
        //Repaint();
    }

    void DeleteLinks()
    {
        Debug.Log("delete");
        _linksPool.DeleteLinks();
        SaveMachinery();
        //EditorUtility.SetDirty(target);
    }

    void GenerateCog()
    {
        foreach (var cog in cogs)
        {
            cog.Data.IsChainRelated = isChainRelated;
            EditorUtility.SetDirty(cog.Data);
        }

        if (isChainRelated)
        {
            if (chainData != null)
            {
                chainData.CogAmount = cogs.Length;
                EditorUtility.SetDirty(chainData);
            }
        }

        StartCogSetup();
    }

    void SetCogData(int i)
    {
        //if (cogs[i] == null) return;
        CogData Data = cogs[i].Data;
        if (Data == null)
        {
            Debug.Log(cogs.Length);
            Debug.Log(cogs[i].name);
            return;
        }

        Data.Radius = EditorGUILayout.FloatField("Radius", Data.Radius);
        Data.circularThickness = EditorGUILayout.FloatField("Thickness", Data.circularThickness);
        Data.HoleType = (ChainEnums.HoleType) EditorGUILayout.EnumPopup("Hole Type", Data.HoleType);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Teeth Settings", EditorStyles.boldLabel);
        Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
        Data.ToothGap = EditorGUILayout.FloatField("Tooth Gap", Data.ToothGap);
        Data.Equalize = EditorGUILayout.Toggle("Equal Gaps", Data.Equalize);
        Data.MinGapLimit = EditorGUILayout.FloatField("Min Gap Limit", Data.MinGapLimit);

        EditorUtility.SetDirty(cogs[i].Data);
    }

    //private ChainLink lastLinkPrefab;

    void SetChainData()
    {
        chainData.Type = (ChainEnums.ChainType) EditorGUILayout.EnumPopup("Type", chainData.Type);
        chainData.UpwardsAxis = (ChainEnums.UpAxis) EditorGUILayout.EnumPopup("Upwards Axis", chainData.UpwardsAxis);
        chainData.Unit = EditorGUILayout.FloatField("Unit", chainData.Unit);
        chainData.RadiusOffset =
            EditorGUILayout.FloatField("Radius Offset",
                chainData.RadiusOffset); //todo: adı cog offset olarak değiştirilebilir
        chainData.Tension = EditorGUILayout.FloatField("Tension", chainData.Tension);

        chainData.LinksPoolPrefab = (LinksPool) EditorGUILayout.ObjectField("Links Pool Prefab",
            chainData.LinksPoolPrefab, typeof(LinksPool), false);

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Link Pool Changed", narrowButton))
            HandlePoolChange();
        GUILayout.FlexibleSpace();

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
            chainData.FollowGearRotation = EditorGUILayout.Toggle("Follow Cog Rotation", chainData.FollowGearRotation);
            chainData.SetMotionByGear = EditorGUILayout.Toggle("Set Motion By Cog", chainData.SetMotionByGear);
        }
    }

    void HandlePoolChange()
    {
        DeleteLinks();
        GetLinkPool();
        GenerateChain();
    }


    void StartCogSetup()
    {
        ChainEvents.OnCogSetupRequest.Invoke();
    }
}

// void NewPool()
// {
//     var go = new GameObject("LinksPool");
//     go.transform.SetParent(_chainSpawner.transform);
//     go.AddComponent<LinksPool>();
//     _linksPool = go.GetComponent<LinksPool>();
//     //ChainEvents.OnLinksPoolUpdated?.Invoke(_linksPool);
//     _chainDrawer.GetLinksPool(_linksPool);
//     SaveMachinery();
// }