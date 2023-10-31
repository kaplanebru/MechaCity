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
    
    //private SerializedProperty cogsArray;
    [SerializeField] private Cogwheel[] cogs;
    private List<CogData> cogDatas = new();
    private string[] cogHolderLabels;
    private int selectedIndex = 0;

    [SerializeField] private ChainData chainData;
    
    public Machinery machineryPrefab;

    
    

    [MenuItem("Tools/Chain Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChainEditorWindow));
    }

    private void OnEnable()
    {
       // _machinery = FindObjectOfType<Machinery>().gameObject;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        serializedObject = new SerializedObject(this);
        //cogsArray = serializedObject.FindProperty("cogs");

         //cogs = FindObjectsOfType<Cogwheel>(); //TODO: TEMP
     
        if(cogs != null) //TODO: possible bug, cog yoksa burda olmaması lazım
            chainData.CogAmount = cogs.Length;
        
    }

    private void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            cogs = chainData.cogs.ToArray(); //FindObjectsOfType<Cogwheel>(); //
            Debug.Log(chainData.cogs.Count);
            Repaint();
        }
    }

    private void OnGUI()
    {
        serializedObject.Update();
        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);
        // EditorGUILayout.PropertyField(cogsArray, true);
        //
        // if (cogs == null || cogs.Length == 0)
        // {
        //     serializedObject.ApplyModifiedProperties();
        //     return;
        // }
        //
        // for (var i = 0; i < cogs.Length; i++)
        // {
        //     var cog = cogs[i];
        //     if (cog == null)
        //     {
        //         serializedObject.ApplyModifiedProperties();
        //         return;
        //         if (chainData.cogs.Count == 0)
        //         {
        //             Debug.Log("cog null");
        //             serializedObject.ApplyModifiedProperties();
        //             return;
        //         }
        //       
        //         cog = chainData.cogs[i];
        //         
        //     }
        //    
        // }


        EditorGUI.BeginChangeCheck();

        machineryPrefab =  (Machinery)EditorGUILayout.ObjectField("Machinery Prefab", machineryPrefab, typeof(Machinery), true);
        if(machineryPrefab == null)
            return;

        if(cogs == null)
            cogs = machineryPrefab.GetComponentsInChildren<Cogwheel>();


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
            foreach (var cog in cogs)
            { 
                EditorUtility.SetDirty(cog.Data);
            }
            chainData.cogs = cogs.ToList();
            StartMachinery();
            EditorUtility.SetDirty(chainData);
            if (machineryPrefab != null)
            {
                Undo.RecordObject(machineryPrefab, "machineryPB");
                EditorUtility.SetDirty(machineryPrefab);
                Repaint();
            }

        }

        if (GUILayout.Button("Delete Teeth"))
        {
            foreach (var cog in cogs)
            {
                var teeth = cog.GetComponent<TeethGenerator>();
                teeth.DeleteTeeth();
            }
        }
       
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("_______________Chain Properties_______________", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        chainData = (ChainData) EditorGUILayout.ObjectField("Chain Data", chainData, typeof(ChainData), false);
        if (chainData != null)
        {
            ChainSettings();
        }

        EditorGUILayout.Space();

        

        
        if (EditorGUI.EndChangeCheck()) //(GUI.changed) 
        {
            // if (cogs[selectedIndex].Data != null)
            // {
            //     EditorUtility.SetDirty(cogs[selectedIndex].Data);
            // }
            //
            // if (chainData != null)
            // {
            //     chainData.CogAmount = cogs.Length;
            //     EditorUtility.SetDirty(chainData);
            // }
            //
            // Undo.RecordObject(this, "Chain Editor");
            // Repaint();
        }

        //serializedObject.ApplyModifiedProperties();
        
        
    }

    void SetCogData(int i)
    {
        //if (cogs[i] == null) return;
        CogData Data = cogs[i].Data;
        cogDatas.Add(Data);
        Data.Radius = EditorGUILayout.FloatField("Radius", Data.Radius);
        Data.circularThickness = EditorGUILayout.FloatField("Thickness", Data.circularThickness);
        
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Teeth Settings", EditorStyles.boldLabel);
        Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
        Data.ToothGap = EditorGUILayout.FloatField("Tooth Gap", Data.ToothGap);
        Data.Equalize = EditorGUILayout.Toggle("Equal Gaps", Data.Equalize);
        Data.MinGapLimit = EditorGUILayout.FloatField("Min Gap Limit", Data.MinGapLimit);
        Data.ToothPoolCount = EditorGUILayout.IntField("Tooth Pool Count", Data.ToothPoolCount);

        EditorUtility.SetDirty(cogs[i].Data);
    }

    void ChainSettings()
    {
        chainData.Type = (ChainEnums.ChainType) EditorGUILayout.EnumPopup("Type", chainData.Type);
        chainData.UpwardsAxis = (ChainEnums.UpAxis) EditorGUILayout.EnumPopup("Upwards Axis", chainData.UpwardsAxis);
        chainData.Unit = EditorGUILayout.FloatField("Unit", chainData.Unit);
        chainData.RadiusOffset = EditorGUILayout.FloatField("Radius Offset", chainData.RadiusOffset); //todo: adı cog offset olarak değiştirilebilir
        chainData.Tension = EditorGUILayout.FloatField("Tension", chainData.Tension);

        chainData.SetRadiusByGear = EditorGUILayout.Toggle("Set Radius By Cog", chainData.SetRadiusByGear);
        chainData.IsMoving = EditorGUILayout.Toggle("Is Moving", chainData.IsMoving);
        
        if (chainData.IsMoving)
        {
            chainData.MachinerySpeed = EditorGUILayout.FloatField("Machinery Speed", chainData.MachinerySpeed);
            chainData.SpeedMultiplier = EditorGUILayout.FloatField("Speed Multiplier", chainData.SpeedMultiplier); 
            chainData.LinkRotationMultiplier = EditorGUILayout.FloatField("Link Rotation Multiplier", chainData.LinkRotationMultiplier);
            
            chainData.motionDirection = (ChainEnums.ChainDirection)EditorGUILayout.EnumPopup("Motion Direction", chainData.motionDirection);
            chainData.FollowGearRotation = EditorGUILayout.Toggle("Follow Cog Rotation", chainData.FollowGearRotation);
            chainData.SetMotionByGear = EditorGUILayout.Toggle("Set Motion By Cog", chainData.SetMotionByGear);
        }
        
        //TODO: COG SPEED BURAYA. SYSTEM SPEED FALAN DA OLABİLİR ADI
    }
    


    void StartMachinery()
    {
        ChainEvents.OnCogSetupRequest.Invoke();
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

    }
}