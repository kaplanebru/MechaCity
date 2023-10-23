using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

public class ChainEditorWindow : EditorWindow
{
   
    private SerializedObject serializedObject;
    private SerializedProperty array;
    public CogHolder[] cogHolders;
    
    // private string scriptableObjectPath = "Assets/GameData/Chain/EditorEventHandler.asset";
    // private EditorEventHandler EventData;
    public CogData cogDataSO;


    private float arcRadius;
    //private Cogwheel cog;

    [MenuItem("Tools/Chain Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChainEditorWindow));
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        array = serializedObject.FindProperty("cogHolders");
    }

    private void OnGUI()
    {
        serializedObject.Update();


        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);
        //cog = EditorGUILayout.ObjectField("Cog", cog, typeof(Cogwheel), true) as Cogwheel;
        cogDataSO = (CogData)EditorGUILayout.ObjectField("ScriptableObject", cogDataSO, typeof(CogData), false);
        
        EditorGUI.BeginChangeCheck();

        // Create fields for the data you want to edit
        cogDataSO.Radius = EditorGUILayout.FloatField("Radius", cogDataSO.Radius);
        cogDataSO.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", cogDataSO.toothScale);
        cogDataSO.circularThickness = EditorGUILayout.FloatField("Thickness", cogDataSO.circularThickness);
            
        cogDataSO.cogObject = (Transform)EditorGUILayout.ObjectField("Cog Object", cogDataSO.cogObject, typeof(Transform), true);

        if (EditorGUI.EndChangeCheck())
        {
            // Save the changes made in the editor
            EditorUtility.SetDirty(cogDataSO);
        }


        EditorGUILayout.PropertyField(array, true);
        

        GUILayout.Label("Cog Settings", EditorStyles.boldLabel);
        
       
        if (GUILayout.Button("Generate Chain"))
        {
            SetCogs();
            
        }
        serializedObject.ApplyModifiedProperties();
    }



    void SetCogs()
    {
        // foreach (var cogHolder in cogHolders)
        // {
        //     cogHolder.SetCogData();
        // }
        ChainEvents.OnCogSetupRequest.Invoke(cogDataSO);
        
       
    }
   
}