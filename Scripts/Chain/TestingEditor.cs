using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Testing))]
public class TestingEditor : Editor
{
    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();

        var testing = target as Testing;

        EditorGUI.BeginChangeCheck();

        if (GUILayout.Button("AddtoList"))
        {
            testing.testObjects.Add(Instantiate(testing.testObj, testing.transform));
        }
    }

}
