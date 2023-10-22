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
        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);
        arcRadius = EditorGUILayout.FloatField("Arc Radius", arcRadius);
        //cog = EditorGUILayout.ObjectField("Cog", cog, typeof(Cogwheel), true) as Cogwheel;

        EditorGUILayout.PropertyField(array, true);
        

       
        if (GUILayout.Button("Generate Chain"))
        {
            DoSth();
        }
        serializedObject.ApplyModifiedProperties();
    }

    void DoSth()
    {
        foreach (var cog in cogArray)
        {
            //cog.Data.Radius = arcRadius;
            //ChainEvents.OnTest.Invoke();
            cog.transform.localScale = arcRadius * 2 * Vector3.one;
            Debug.Log("yo");
        }
    }
   
}