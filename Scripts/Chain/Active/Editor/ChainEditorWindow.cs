using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chain;
using MyNamespace;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class ChainEditorWindow : EditorWindow
{
    private SerializedObject serializedObject;

    [SerializeField] private Cogwheel[] cogs;
    private string[] cogHolderLabels;
    private int selectedIndex = 0;

    [SerializeField] private bool isChainRelated;

    [SerializeField] private ChainData chainData;

    public Machinery machineryPrefab;


    [MenuItem("Tools/Chain Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChainEditorWindow));
    }

    private LinksPool _linksPool;
    

    private void OnGUI()
    {
        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);


        EditorGUI.BeginChangeCheck();

        machineryPrefab =
            (Machinery) EditorGUILayout.ObjectField("Machinery Prefab", machineryPrefab, typeof(Machinery), true);
        if (machineryPrefab == null)
            return;

        if (cogs == null || (cogs.Length > 0 && cogs[0] == null))
            cogs = machineryPrefab.GetComponentsInChildren<Cogwheel>();

        if (_linksPool == null)
            _linksPool = machineryPrefab.GetComponentInChildren<LinksPool>();

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
            SaveMachinery();
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

            //if(machineryPrefab.GetComponentInChildren<ChainSpawner>().Data == null) TODO: yoksa yarat, varsa get
            chainData = (ChainData) EditorGUILayout.ObjectField("Chain Data", chainData, typeof(ChainData), false);
            if (chainData != null)
            {
                ChainSettings();
                
                if (GUILayout.Button("Generate Chain"))
                {
                   GenerateChain();
                   SaveMachinery();

                }

                if (GUILayout.Button("DeleteLinks"))
                {
                    DeleteLinks();
                }
                
            }
        }


        EditorGUILayout.Space();


        if (EditorGUI.EndChangeCheck()) //(GUI.changed) 
        {
        }
    }

    void SaveMachinery()
    {
        if (machineryPrefab != null)
        {
            Undo.RecordObject(machineryPrefab, "machineryPB");
            EditorUtility.SetDirty(machineryPrefab);
            Repaint();
        }
    }

    void GenerateChain()
    {
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
        
        _linksPool.DeleteLinks();
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

    void ChainSettings()
    {
        chainData.Type = (ChainEnums.ChainType) EditorGUILayout.EnumPopup("Type", chainData.Type);
        chainData.UpwardsAxis = (ChainEnums.UpAxis) EditorGUILayout.EnumPopup("Upwards Axis", chainData.UpwardsAxis);
        chainData.Unit = EditorGUILayout.FloatField("Unit", chainData.Unit);
        chainData.RadiusOffset =
            EditorGUILayout.FloatField("Radius Offset",
                chainData.RadiusOffset); //todo: adı cog offset olarak değiştirilebilir
        chainData.Tension = EditorGUILayout.FloatField("Tension", chainData.Tension);

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