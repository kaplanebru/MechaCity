using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEditor;
using UnityEngine;

public class ChainEditorWindow : EditorWindow
{
   
    private SerializedObject serializedObject;
    private SerializedProperty array;
    public Cogwheel[] cogArray;
    
    // private string scriptableObjectPath = "Assets/GameData/Chain/EditorEventHandler.asset";
    // private EditorEventHandler EventData;
    


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
        array = serializedObject.FindProperty("cogArray");
    }

    private void OnGUI()
    {
        serializedObject.Update();
        
        // EventData = AssetDatabase.LoadAssetAtPath<EditorEventHandler>(scriptableObjectPath);
        // Debug.Log(EventData.name);

        
        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);
        arcRadius = EditorGUILayout.FloatField("Arc Radius", arcRadius);
        //cog = EditorGUILayout.ObjectField("Cog", cog, typeof(Cogwheel), true) as Cogwheel;

        EditorGUILayout.PropertyField(array, true);
        

        SetCogs();
       
        if (GUILayout.Button("Generate Chain"))
        {
            DoSth();
            Debug.Log("log");
            ChainEvents.OnTest.Invoke();
            //EventData.RaiseEvent();
        }
        serializedObject.ApplyModifiedProperties();
    }

    void DoSth()
    {
        foreach (var cog in cogArray)
        {
            //cog.Data.Radius = arcRadius;
           
            //cog.transform.localScale = arcRadius * 2 * Vector3.one;
            //Debug.Log("yo");
        }
    }

    void SetCogs()
    {
        GUILayout.Label("Cog Settings", EditorStyles.boldLabel);
    }
   
}