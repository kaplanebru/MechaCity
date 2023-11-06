using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private bool isChainRelated;
    [SerializeField] private bool chooseAnotherData;
    [SerializeField] private bool newChainData;
    [SerializeField] private ChainData oldChainData;
    //[SerializeField] private LinksPool linksPoolPrefab;

    [SerializeField] private ChainData chainData;
    private LinksPool _linksPool;
    private ChainSpawner _chainSpawner;
    private ChainDrawer _chainDrawer;

    public Machinery machineryPrefab;


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the default Inspector

        machineryPrefab = target as Machinery;
        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);

        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("SAVE CHANGES"))
        {
            SaveMachinery();
        }

        if (GUILayout.Button("OVERRIDE CHANGES ON SCENE"))
        {
            SaveMachinery();
            OverrideChanges();
        }


        if (cogs == null || (cogs.Length > 0 && cogs[0] == null))
            cogs = machineryPrefab.GetComponentsInChildren<Cogwheel>();


      

        isChainRelated = EditorGUILayout.Toggle("Is Chain Related", isChainRelated);

        GUILayout.Label(" _______________Cog Settings_______________ ", EditorStyles.boldLabel); //\n 
        EditorGUILayout.Space();
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
        {
            GenerateCog();
            //SaveMachinery();
        }

        if (GUILayout.Button("Delete Teeth"))
        {
            foreach (var cog in cogs)
            {
                var teeth = cog.GetComponent<TeethGenerator>();
                teeth.DeleteTeeth();
            }
        }

        if (isChainRelated)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("_______________Chain Properties_______________", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (_chainSpawner == null)
                _chainSpawner = machineryPrefab.GetComponentInChildren<ChainSpawner>();
            else
            {
                chainData = (ChainData) EditorGUILayout.ObjectField("Chain Data", chainData, typeof(ChainData), false);
            }
            

            if (_chainDrawer == null)
                _chainDrawer = machineryPrefab.GetComponentInChildren<ChainDrawer>();

            chooseAnotherData = EditorGUILayout.Toggle("Choose Another Data", chooseAnotherData);
            newChainData = EditorGUILayout.Toggle("Create New Chain Data", newChainData);

            if (chainData == null && !chooseAnotherData)
            {
                if (_chainSpawner.Data != null)
                {
                    chainData = _chainSpawner.Data;
                }

                oldChainData = chainData;
            }

            if (chooseAnotherData)
            {
                // if (chainData != oldChainData)
                // {
                chainData = (ChainData) EditorGUILayout.ObjectField("Chain Data", chainData, typeof(ChainData), false);
                // }
            }


            if (newChainData)
            {
                if (GUILayout.Button("Create New Chain Data"))
                    CreateChainData();
            }


            if (chainData != null)
            {
                ChainSettings();
                if (_linksPool == null)
                {
                    _linksPool = machineryPrefab.GetComponentInChildren<LinksPool>();
                    if (_linksPool == null)
                    {
                        _linksPool = Instantiate(chainData.LinksPoolPrefab, _chainSpawner.transform);
                       
                    }
                    _chainDrawer.GetLinksPool(_linksPool);
                }

                if (GUILayout.Button("Generate Chain"))
                {
                    GenerateChain();
                    //SaveMachinery();
                }

                if (GUILayout.Button("DeleteLinks"))
                {
                    DeleteLinks();
                    //NewPool();
                }

                if (GUILayout.Button("create POOL"))
                {
                    Debug.Log("NEWPOOL");
                    NewPool();
                }
            }
        }


        EditorGUILayout.Space();

        EditorGUI.EndChangeCheck();
        // if (EditorGUI.EndChangeCheck()) //(GUI.changed) 
        // {
        // }
    }


    void NewPool()
    {
        var go = new GameObject("LinksPool");
        go.transform.SetParent(_chainSpawner.transform);
        go.AddComponent<LinksPool>();
        _linksPool = go.GetComponent<LinksPool>();
        //ChainEvents.OnLinksPoolUpdated?.Invoke(_linksPool);
        _chainDrawer.GetLinksPool(_linksPool);
        SaveMachinery();
    }

    void CreateChainData()
    {
        chainData = CreateInstance<ChainData>();
        //var allChainDatas = Resources.LoadAll<ChainData>("ChainDatas");
        string[] guids = AssetDatabase.FindAssets("t:ChainData");
        int newIndex = guids.Length + 1;
        AssetDatabase.CreateAsset(chainData,
            GetPath(nameof(chainData) + newIndex,
                "ChainDatas")); //(chainData, "Assets/chainData.asset"); //TODO ismine +1 eklenir foldera bakılıp
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    string GetPath(string fileName, string subFolderName)
    {
        string basePath = "Assets/Resources/" + subFolderName;
        return Path.Combine(basePath, fileName + ".asset");
    }

    void SaveMachinery()
    {
        if(isChainRelated)
            _chainSpawner.Data = chainData;
        
        EditorUtility.SetDirty(target);

        //PrefabUtility.SavePrefabAsset(target as GameObject);
        // if (machineryPrefab != null)
        // {
        //     EditorUtility.SetDirty(target);
        //
        //     // Automatically save the prefab
        //     if (target is GameObject) // Check if target is a GameObject
        //     {
        //         GameObject prefabObject = (GameObject)target;
        //         PrefabUtility.SavePrefabAsset(prefabObject);
        //     }
        //     // Undo.RecordObject(machineryPrefab, "machineryPB");
        //     // EditorUtility.SetDirty(machineryPrefab);
        //     // PrefabUtility.SavePrefabAsset(machineryPrefab.gameObject);
        //     Repaint();
        // }
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

                if (lastLinkPrefab != null && lastLinkPrefab != chainData.linkPrefab) // //TODO: test later
                    DeleteLinks();
                lastLinkPrefab = chainData.linkPrefab;

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

    private ChainLink lastLinkPrefab;

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

        
        chainData.linkPrefab =
            (ChainLink) EditorGUILayout.ObjectField("Link Prefab", chainData.linkPrefab, typeof(ChainLink), false);

        

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


    void StartCogSetup()
    {
        ChainEvents.OnCogSetupRequest.Invoke();
    }
}