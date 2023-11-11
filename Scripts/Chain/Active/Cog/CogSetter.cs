using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class CogSetter : MonoBehaviour
    {
        private CogData Data;
        private Cogwheel cog;

        private void OnEnable()
        {
            cog = GetComponent<Cogwheel>();
            Data = cog.Data;

            ChainEvents.OnNewCogData += SetData;
            ChainEvents.OnCogSetupRequest += Setup;
        }

        private void SetData(CogData data)
        {
            Data = data;
        }


        void Setup()
        {
            Data = cog.Data;
            var radius = Data.Radius;
            var scale = Vector3.one;
            scale.x = radius * 2;

            if (ChainSpawner.Upwards == ChainEnums.UpAxis.Z)
                scale.z = radius * 2;
            else
                scale.y = radius * 2;

            cog.cogObject.transform.localScale = scale;

            SetHoleSize();

            ChainEvents.OnCogDataSet?.Invoke(Data, transform);
            
            
            //ChainEvents.OnCogStart?.Invoke(Data, teeth);
        }

        Hole[] GetHolesByType(ChainEnums.HoleType holeType)
        {
            var allHoles = GetComponentsInChildren<Hole>(true);
            return allHoles.Where(h =>
            {
                h.gameObject.SetActive(h.holeType == holeType);
                return h.holeType == holeType;
            }).ToArray();
        }
        void SetHoleSize()
        {
            Hole[] holes = GetHolesByType(Data.HoleType);
            var holeSize = (Data.Radius - Data.circularThickness) * 2;
            foreach (var hole in holes)
            {
                // Vector3 inverseParentScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y,
                //     1f / transform.localScale.z);

                Vector3 scale = hole.transform.localScale;

                scale.x = holeSize;
                if (ChainSpawner.Upwards == ChainEnums.UpAxis.Z)
                    scale.z = holeSize;
                else
                    scale.y = holeSize;

                scale.x = holeSize;

                hole.transform.localScale = scale; //Vector3.Scale(scale, inverseParentScale);
            }
        }

        private void OnDisable()
        {
            ChainEvents.OnCogSetupRequest -= Setup;
            ChainEvents.OnNewCogData -= SetData;

        }
    }
}