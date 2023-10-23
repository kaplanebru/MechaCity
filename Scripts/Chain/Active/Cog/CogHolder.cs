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
        public CogData Data;

        public Cogwheel Cog;
        //public Transform CogObject;

        public void SetCogData()
        {
            //Cog.Data = Data;
            //Data = (CogData) EditorGUILayout.ObjectField("Cog Data", Data, typeof(CogData), false);
            Data.Radius = EditorGUILayout.FloatField("Radius", Data.Radius);
            Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
            Data.circularThickness = EditorGUILayout.FloatField("Thickness", Data.circularThickness);
            Data.cogObject =
                (Transform) EditorGUILayout.ObjectField("Cog Object", Data.cogObject, typeof(Transform), true);

            Debug.Log("set cog data");
        }
        
    }
}