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
    private SerializedProperty cogHoldersArray;
    private SerializedProperty cogDatasArray;
    public CogHolder[] cogHolders;

    private int selectedIndex = 0;


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
        cogHoldersArray = serializedObject.FindProperty("cogHolders");
        //cogDatasArray = serializedObject.FindProperty("cogDatas");
    }

    private void OnGUI()
    {
        serializedObject.Update();

        if (cogHolders == null)
        {
            EditorGUILayout.HelpBox("Assign the CogHolders array in the Inspector.", MessageType.Info);
            return;
        }

        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);


        EditorGUI.BeginChangeCheck();

        selectedIndex = EditorGUILayout.IntSlider("Selected CogHolder", selectedIndex, 0, cogHolders.Length - 1);
        if (selectedIndex >= 0 && selectedIndex < cogHolders.Length)
        {
            EditorGUI.indentLevel++;
            cogHolders[selectedIndex].SetCogData();
            EditorGUI.indentLevel--;
        }

        // foreach (var cogHolder in cogHolders)
        // {
        //     cogHolder.SetCogData();
        // }

        // foreach (var cogHolder in cogHolders)
        // {
        //     cogHolder.Data.Radius = EditorGUILayout.FloatField("Radius",  cogHolder.Data.Radius);
        //     cogHolder.Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale",  cogHolder.Data.toothScale);
        //     cogHolder.Data.circularThickness = EditorGUILayout.FloatField("Thickness",  cogHolder.Data.circularThickness);
        //     cogHolder.Data.cogObject = (Transform)EditorGUILayout.ObjectField("Cog Object",  cogHolder.Data.cogObject, typeof(Transform), true);
        // }


        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(cogHolders[selectedIndex].Data);
        }


        EditorGUILayout.PropertyField(cogHoldersArray, true);


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