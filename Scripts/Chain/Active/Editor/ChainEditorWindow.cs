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
    private string[] cogHolderLabels;

    private int selectedIndex = 0;

    [MenuItem("Tools/Chain Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChainEditorWindow));
    }

    private void OnEnable()
    {
        serializedObject = new SerializedObject(this);
        cogHoldersArray = serializedObject.FindProperty("cogHolders");
    }

    private void OnGUI()
    {
        serializedObject.Update();


        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);
        
       
        
       
        
        EditorGUILayout.PropertyField(cogHoldersArray, true);

        if (cogHolders == null)
        {
            EditorGUILayout.HelpBox("Assign the CogHolders array in the Inspector.", MessageType.Info);
            return;
        }

        EditorGUI.BeginChangeCheck();

        GUILayout.Label("_____Cog Settings_____", EditorStyles.boldLabel); //\n 
        if (cogHolderLabels == null || cogHolderLabels.Length != cogHolders.Length)
        {
            cogHolderLabels = new string[cogHolders.Length];
            for (int i = 0; i < cogHolders.Length; i++)
            {
                cogHolderLabels[i] = "Cog " + i;
            }
        }

        selectedIndex = EditorGUILayout.Popup("Selected Cog", selectedIndex, cogHolderLabels);

        if (selectedIndex >= 0 && selectedIndex < cogHolders.Length)
        {
            EditorGUI.indentLevel++;
            cogHolders[selectedIndex].SetCogData();
            EditorGUI.indentLevel--;
        }


        if (GUI.changed) //(EditorGUI.EndChangeCheck())
        {
            if (cogHolders[selectedIndex].cog.Data != null)
            {
                EditorUtility.SetDirty(cogHolders[selectedIndex].cog.Data);
            }
        }


      


        if (GUILayout.Button("Generate Chain"))
        {
            SetCogs();
        }

        serializedObject.ApplyModifiedProperties();
    }


    void SetCogs()
    {
        ChainEvents.OnCogSetupRequest.Invoke();
    }
}