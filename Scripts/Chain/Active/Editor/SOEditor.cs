using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    public class SOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CogData cogDataSO = (CogData)target;

            EditorGUI.BeginChangeCheck();

            // Create fields for the data you want to edit
            cogDataSO.Radius = EditorGUILayout.FloatField("Radius", cogDataSO.Radius);
            cogDataSO.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", cogDataSO.toothScale);
            cogDataSO.circularThickness = EditorGUILayout.FloatField("Thickness", cogDataSO.circularThickness);
            
            cogDataSO.cogObject = (Transform)EditorGUILayout.ObjectField("Transform Field", cogDataSO.cogObject, typeof(Transform), true);

            if (EditorGUI.EndChangeCheck())
            {
                // Save the changes made in the editor
                EditorUtility.SetDirty(cogDataSO);
            }
        }
    }

}
