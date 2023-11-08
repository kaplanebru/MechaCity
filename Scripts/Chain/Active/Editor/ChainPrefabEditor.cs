using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using Chain;
using MyNamespace;
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
    [SerializeField] private Cogwheel cogPrefab;
    private LinksPool _linksPool;
    private ChainSpawner _chainSpawner;
    private ChainDrawer _chainDrawer;

    public Machinery machineryPrefab;
    
    private GUIStyle narrowButton;

    [SerializeField]int destroyCogIndex;

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
            cogs = machineryPrefab.GetComponentsInChildren<Cogwheel>();

        
        SetChainRelation();

        GUILayout.Label(" _______________Cog Settings_______________ ", EditorStyles.boldLabel); //\n 
        EditorGUILayout.Space();
        
        cogPrefab = (Cogwheel) EditorGUILayout.ObjectField("Cog Prefab", cogPrefab, typeof(Cogwheel), false);
        
        
        destroyCogIndex = EditorGUILayout.IntField("destroy cog index", destroyCogIndex);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Cog"))
        {
            Instantiate(cogPrefab, machineryPrefab.transform.GetChild(0).transform); //TODO: address properly
            cogs = machineryPrefab.GetComponentsInChildren<Cogwheel>();
            ChainEvents.OnCogsUpdated?.Invoke(cogs);
        }
        
        if (GUILayout.Button("Remove Cog"))
        {
            //DestroyImmediate(cogs[cogIndex].gameObject);
            if(cogs.Length == 0 || cogs == null) return;
            cogs[destroyCogIndex].gameObject.SetActive(false);
            cogs = machineryPrefab.GetComponentsInChildren<Cogwheel>();
            ChainEvents.OnCogsUpdated?.Invoke(cogs);
            machineryPrefab.GetComponentInChildren<ChainDrawer>().ResetLinks();
        }
        
        GUILayout.EndHorizontal();
        
        if (cogHolderLabels == null || cogHolderLabels.Length != cogs.Length)
        {
            cogHolderLabels = new string[cogs.Length];
            for (int i = 0; i < cogs.Length; i++)
            {
                cogHolderLabels[i] = "Cog " + i;
            }
        }

        selectedIndex = EditorGUILayout.Popup("Selected Cog", selectedIndex, cogHolderLabels);

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
                var teeth = cog.GetComponent<TeethGenerator>();
                teeth.DeleteTeeth();
            }
        }
        

        if (machineryPrefab.isChainRelated)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("_______________Chain Properties_______________", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (_chainSpawner == null)
                _chainSpawner = machineryPrefab.GetComponentInChildren<ChainSpawner>();
            
            if (_chainDrawer == null)
                _chainDrawer = machineryPrefab.GetComponentInChildren<ChainDrawer>();
            
            if (chainData == null) //Get old data
            {
                if (_chainSpawner.Data != null)
                    chainData = _chainSpawner.Data;
            }

            newChainData = EditorGUILayout.Toggle("Create New Chain Data", newChainData);
            chainData = (ChainData) EditorGUILayout.ObjectField("Chain Data", chainData, typeof(ChainData), false);

            if (newChainData)
            {
                chainDataName = EditorGUILayout.TextField("Chain Data Name", chainDataName); //write the same name if you want to modify pool + reset pool before

                if (GUILayout.Button("Create New Chain Data"))
                    CreateChainData();
            }


            if (chainData != null)
            {
                ChainSettings();
                GetLinkPool();

                if (GUILayout.Button("Generate Chain"))
                    GenerateChain();
                

                if (GUILayout.Button("Delete Link Pool"))
                    DeleteLinks();

                if (GUILayout.Button("Deactivate Link pool"))
                {
                    _chainDrawer.ResetLinks();
                }
            }
        }
        
        EditorGUILayout.Space();
        EditorGUI.EndChangeCheck();
    }
    
   
    

    void GetLinkPool()
    {
        if (_linksPool == null)
        {
            _linksPool = machineryPrefab.GetComponentInChildren<LinksPool>();
            if (_linksPool == null)
            {
                _linksPool = Instantiate(chainData.LinksPoolPrefab, _chainSpawner.transform);
                       
            }
            _chainDrawer.GetLinksPool(_linksPool);
        }
    }

    void SetChainRelation()
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

    void CreateChainData()
    {
        chainData = CreateInstance<ChainData>();
        
        AssetDatabase.CreateAsset(chainData, MyEditorHelpers.WriteAssetPath(chainDataName, "ChainDatas")); //(chainData, "Assets/chainData.asset"); //TODO ismine +1 eklenir foldera bakılıp
        Debug.Log(MyEditorHelpers.WriteAssetPath(chainDataName, "ChainDatas"));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    

    void SaveMachinery()
    {
        if(isChainRelated)
            _chainSpawner.Data = chainData;
        
        EditorUtility.SetDirty(target);
    }

    void OverrideChanges()
    {
        PrefabUtility.ApplyPrefabInstance(machineryPrefab.gameObject, InteractionMode.UserAction);
    }

    void GenerateChain()
    {
        machineryPrefab.GetComponentInChildren<ChainSpawner>().Data = chainData;
        foreach (var cog in cogs)
        {
            cog.Data.IsMoving = chainData.IsMoving;
        }

        GenerateCog();

        ChainEvents.OnChainRequest?.Invoke(); //ninvoke pas en enable
        //Repaint();
        //_linksPool.transform.position = Vector3.zero;
       // _linksPool.transform.rotation = Quaternion.identity;
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

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Teeth Settings", EditorStyles.boldLabel);
        Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
        Data.ToothGap = EditorGUILayout.FloatField("Tooth Gap", Data.ToothGap);
        Data.Equalize = EditorGUILayout.Toggle("Equal Gaps", Data.Equalize);
        Data.MinGapLimit = EditorGUILayout.FloatField("Min Gap Limit", Data.MinGapLimit);

        EditorUtility.SetDirty(cogs[i].Data);
    }

    //private ChainLink lastLinkPrefab;

    void ChainSettings()
    {
        chainData.Type = (ChainEnums.ChainType) EditorGUILayout.EnumPopup("Type", chainData.Type);
        chainData.UpwardsAxis = (ChainEnums.UpAxis) EditorGUILayout.EnumPopup("Upwards Axis", chainData.UpwardsAxis);
        chainData.Unit = EditorGUILayout.FloatField("Unit", chainData.Unit);
        chainData.RadiusOffset =
            EditorGUILayout.FloatField("Radius Offset",
                chainData.RadiusOffset); //todo: adı cog offset olarak değiştirilebilir
        chainData.Tension = EditorGUILayout.FloatField("Tension", chainData.Tension);
        
        chainData.LinksPoolPrefab = (LinksPool) EditorGUILayout.ObjectField("Links Pool Prefab",  chainData.LinksPoolPrefab, typeof(LinksPool), false);
        
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