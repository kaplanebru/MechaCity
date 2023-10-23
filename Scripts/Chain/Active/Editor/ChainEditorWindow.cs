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
    private SerializedProperty cogsArray;
    private SerializedProperty cogDatasArray;
    
    [SerializeField]
    public Cogwheel[] cogs;
    private List<CogData> cogDatas = new();
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
        cogsArray = serializedObject.FindProperty("cogs");
    }

    private void OnGUI()
    {
        serializedObject.Update();


        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);


        EditorGUILayout.PropertyField(cogsArray, true);

        if (cogs == null || cogs.Length == 0)
        {
            EditorGUILayout.HelpBox("Assign the CogHolders array in the Inspector.", MessageType.Info);
            serializedObject.ApplyModifiedProperties();
            return;
        }
        

        EditorGUI.BeginChangeCheck();

        GUILayout.Label("_____Cog Settings_____", EditorStyles.boldLabel); //\n 
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
            if (cogs[selectedIndex] == null)
            {
                Debug.Log(selectedIndex + " is null");
                
                // serializedObject.ApplyModifiedProperties();
                // Repaint();
                // return;
            }
              
            EditorGUI.indentLevel++;
            SetCogData(selectedIndex);
            EditorGUI.indentLevel--;
            
        }


        if (EditorGUI.EndChangeCheck()) //(GUI.changed) 
        {
            if (cogs[selectedIndex].Data != null)
            {
                EditorUtility.SetDirty(cogs[selectedIndex].Data);
            }
        }


        if (GUILayout.Button("Generate Chain"))
        {
            SetCogs();
        }

        serializedObject.ApplyModifiedProperties();
    }

    public void SetCogData(int i)
    {
        //if (cogs[i] == null) return;
        CogData Data = cogs[i].Data;
        cogDatas.Add(Data);
        Data.Radius = EditorGUILayout.FloatField("Radius", Data.Radius);
        Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
        Data.circularThickness = EditorGUILayout.FloatField("Thickness", Data.circularThickness);
        Data.cogObject = cogs[i].cogObject;

        EditorUtility.SetDirty(cogs[i].Data);
    }
    


    void SetCogs()
    {
        ChainEvents.OnCogSetupRequest.Invoke();
    }
}