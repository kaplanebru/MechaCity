using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEditor;
using UnityEngine;

public class ChainEditorWindow : EditorWindow
{
    private float arcRadius;
    private Cogwheel cog;
    [MenuItem("Tools/Chain Generator")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ChainEditorWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label("Chain Generator", EditorStyles.boldLabel);
        arcRadius = EditorGUILayout.FloatField("Arc Radius", arcRadius);
        cog = EditorGUILayout.ObjectField("Cog", cog, typeof(Cogwheel), true) as Cogwheel;

        if (GUILayout.Button("Generate Chain"))
        {
            Debug.Log("generate chain");
        }
    }
}
