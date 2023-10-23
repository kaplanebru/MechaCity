using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    [Serializable]
    public class CogHolder
    {
        CogData Data;
        //public Transform CogObject;

        [SerializeField]
        public Cogwheel cog;

        public void SetCogData()
        {
            if (cog == null) return;
            Data = cog.Data;
            Data.Radius = EditorGUILayout.FloatField("Radius", Data.Radius);
            Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
            Data.circularThickness = EditorGUILayout.FloatField("Thickness", Data.circularThickness);
            Data.cogObject = cog.cogObject;
            
            //cog = (Cogwheel)EditorGUILayout.ObjectField("Cog Object", cog, typeof(Cogwheel), true);
            // cog = EditorGUILayout.ObjectField("Cog", cog, typeof(Cogwheel), true) as Cogwheel;
            // Data = (CogData) EditorGUILayout.ObjectField("Cog Data", Data, typeof(CogData), false);
            //Data.cogObject = (Transform) EditorGUILayout.ObjectField("Cog Object", Data.cogObject, typeof(Transform), true);
            //Undo.RecordObject(cog, "Cog");

        }
        
    }
}