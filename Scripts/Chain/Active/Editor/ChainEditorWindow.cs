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

        foreach (var cogHolder in cogHolders)
        {
            cogHolder.Data = (CogData)EditorGUILayout.ObjectField("Cog Data", cogHolder.Data, typeof(CogData), false);
        }
        //cogDataSO = (CogData)EditorGUILayout.ObjectField("ScriptableObject", cogDataSO, typeof(CogData), false);
        
        EditorGUI.BeginChangeCheck();

        foreach (var cogHolder in cogHolders)
        {
            cogHolder.Data.Radius = EditorGUILayout.FloatField("Radius",  cogHolder.Data.Radius);
            cogHolder.Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale",  cogHolder.Data.toothScale);
            cogHolder.Data.circularThickness = EditorGUILayout.FloatField("Thickness",  cogHolder.Data.circularThickness);
            
            cogHolder.Data.cogObject = (Transform)EditorGUILayout.ObjectField("Cog Object",  cogHolder.Data.cogObject, typeof(Transform), true);
        }

       

        if (EditorGUI.EndChangeCheck())
        {
            foreach (var cogHolder in cogHolders)
            {
                EditorUtility.SetDirty(cogHolder.Data);
            }
            
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
        ChainEvents.OnCogSetupRequest.Invoke();
        
       
    }
   
}