using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using MyNamespace;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class ChainEditorWindow : EditorWindow
{

    private SerializedObject serializedObject;
    
    private SerializedProperty cogsArray;
    [SerializeField] private Cogwheel[] cogs;
    [SerializeField] private CogHolder cogHolder;
    private List<CogData> cogDatas = new();
    private string[] cogHolderLabels;
    private int selectedIndex = 0;

    [SerializeField] private ChainData chainData;
    
    

    [MenuItem("Tools/Chain Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChainEditorWindow));
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        cogsArray = serializedObject.FindProperty("cogs");
        cogs = FindObjectsOfType<Cogwheel>();
    }

    private void OnGUI()
    {
        serializedObject.Update();
        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(cogsArray, true);
        
        cogHolder = (CogHolder) EditorGUILayout.ObjectField("Cog Holder", cogHolder, typeof(CogHolder), false);

        if (cogs == null || cogs.Length == 0)
        {
            EditorGUILayout.HelpBox("Assign the CogHolders array in the Inspector.", MessageType.Info);
            serializedObject.ApplyModifiedProperties();
            return;
        }
        

        EditorGUI.BeginChangeCheck();

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
        
        if (GUILayout.Button("Generate Cog"))
        {
            SetCogs();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("_______________Chain Properties_______________", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        chainData = (ChainData) EditorGUILayout.ObjectField("Cog Data", chainData, typeof(ChainData), false);
        if (chainData != null)
        {
            SetChainData();
        }

        
        
        if (EditorGUI.EndChangeCheck()) //(GUI.changed) 
        {
            if (cogs[selectedIndex].Data != null)
            {
                EditorUtility.SetDirty(cogs[selectedIndex].Data);
            }
            
            if(chainData != null)
                EditorUtility.SetDirty(chainData);
        }

        serializedObject.ApplyModifiedProperties();
        
        
    }

    void SetCogHolder()
    {
       // cogHolder.Cogs = cogsArray;

    }

    void SetCogData(int i)
    {
        //if (cogs[i] == null) return;
        CogData Data = cogs[i].Data;
        cogDatas.Add(Data);
        Data.Radius = EditorGUILayout.FloatField("Radius", Data.Radius);
        Data.circularThickness = EditorGUILayout.FloatField("Thickness", Data.circularThickness);
        Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);

        EditorUtility.SetDirty(cogs[i].Data);
    }

    void SetChainData()
    {
        chainData.Type = (ChainEnums.ChainType) EditorGUILayout.EnumFlagsField("Type", chainData.Type);
        chainData.UpwardsAxis = (ChainEnums.UpAxis) EditorGUILayout.EnumFlagsField("Upwards Axis", chainData.UpwardsAxis);
        chainData.Unit = EditorGUILayout.FloatField("Unit", chainData.Unit);
        chainData.RadiusOffset = EditorGUILayout.FloatField("Radius Offset", chainData.RadiusOffset); //todo: adı cog offset olarak değiştirilebilir
        chainData.Tension = EditorGUILayout.FloatField("Tension", chainData.Tension);

        chainData.SetRadiusByGear = EditorGUILayout.Toggle("Set Radius By Cog", chainData.SetRadiusByGear);
        chainData.IsMoving = EditorGUILayout.Toggle("Is Moving", chainData.IsMoving);
        
        if (chainData.IsMoving)
        {
            chainData.Speed = EditorGUILayout.FloatField("Speed Multiplier", chainData.Speed); //todo : bool seçilince gelebilir
            chainData.LinkRotationMultiplier = EditorGUILayout.FloatField("Link Rotation Multiplier", chainData.LinkRotationMultiplier);
            
            chainData.motionDirection = (ChainEnums.ChainDirection)EditorGUILayout.EnumFlagsField("Motion Direction", chainData.motionDirection);
            chainData.FollowGearRotation = EditorGUILayout.Toggle("Follow Cog Rotation", chainData.FollowGearRotation);
            chainData.SetMotionByGear = EditorGUILayout.Toggle("Set Motion By Cog", chainData.SetMotionByGear);
        }
        
        //TODO: COG SPEED BURAYA. SYSTEM SPEED FALAN DA OLABİLİR ADI
    }
    


    void SetCogs()
    {
        ChainEvents.OnCogSetupRequest.Invoke();
    }
}