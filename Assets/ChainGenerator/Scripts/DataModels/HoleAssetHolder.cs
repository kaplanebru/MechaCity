using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [CreateAssetMenu(fileName = nameof(HoleAssetHolder), menuName = "Chain Generator/" + nameof(HoleAssetHolder))]
    public class HoleAssetHolder : ScriptableObject
    {
        public List<Hole> HoleTypes = new List<Hole>();
        public Material HoleMaterial;
        public string[] HoleLabels;

        public void RestoreHoleLabels()
        {
            HoleLabels = new string[HoleTypes.Count + 1];
            for (int i = 0; i < HoleTypes.Count; i++)
            {
                HoleLabels[i] = HoleTypes[i].name;
            }

            HoleLabels[HoleLabels.Length - 1] = "Without Hole";
        }

        public void AddHole(Hole newHole)
        {
            HoleTypes.Add(newHole);
            
            RestoreHoleLabels();
        }
    }

}
