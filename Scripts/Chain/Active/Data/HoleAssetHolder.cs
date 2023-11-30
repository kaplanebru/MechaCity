using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [CreateAssetMenu(fileName = nameof(HoleAssetHolder))]
    public class HoleAssetHolder : ScriptableObject
    {
        public List<Hole> HoleTypes;
        public Material HoleMaterial;
        public string[] HoleLabels;
        public HoleHolder holeHolder;

        public void RestoreHoleLabels()
        {
            HoleLabels = new string[HoleTypes.Count + 1];
            for (int i = 0; i < HoleTypes.Count; i++)
            {
                HoleLabels[i] = HoleTypes[i].name;
            }

            HoleLabels[HoleLabels.Length - 1] = "None";
        }

        // private void OnEnable()
        // {
        //     RestoreHoleLabels();
        // }
    }

}
