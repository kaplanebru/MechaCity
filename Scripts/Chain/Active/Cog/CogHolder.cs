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
        [SerializeField]
        public CogData Data;
        //public Transform CogObject;
        [SerializeField]
        public Cogwheel cog;

        public void SetCogData()
        {
            //cog = (Cogwheel)EditorGUILayout.ObjectField("Cog Object", cog, typeof(Cogwheel), true);
            cog = EditorGUILayout.ObjectField("Cog", cog, typeof(Cogwheel), true) as Cogwheel;
            Data = (CogData) EditorGUILayout.ObjectField("Cog Data", Data, typeof(CogData), false);
            
            if (Data == null || cog == null)
            {
                if(Data == null)
                    Debug.Log("cog data null");
                if(cog == null)
                    Debug.Log("cog null");
                return;
            }
            
            Data.Radius = EditorGUILayout.FloatField("Radius", Data.Radius);
            Data.toothScale = EditorGUILayout.Vector3Field("Tooth Scale", Data.toothScale);
            Data.circularThickness = EditorGUILayout.FloatField("Thickness", Data.circularThickness);
            //Data.cogObject = (Transform) EditorGUILayout.ObjectField("Cog Object", Data.cogObject, typeof(Transform), true);
            

            //Data.cogObject = cog.cogObject;
            Debug.Log("set cog data");
            Undo.RecordObject(cog, "Cog");

        }
        
    }
}