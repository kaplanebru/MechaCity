using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [ExecuteInEditMode]
    public class HoleHolder : MonoBehaviour
    {
        public HoleAssetHolder assetHolder;
        private Hole currentHole;
        private Hole oldHole;
        

        public void ResetHoles()
        {
            foreach (var holeAsset in assetHolder.HoleTypes)
            {
                Instantiate(holeAsset, transform);
            }
            MyPrefabHelpers.ApplyChangesToPrefab(gameObject);
        }

        public Hole CreateHole(int i)
        {
            
            if(currentHole != null)
                 oldHole = currentHole;
            if(i < assetHolder.HoleTypes.Count)
                currentHole = Instantiate(assetHolder.HoleTypes[i], transform);
            if(oldHole != null)
                DestroyImmediate(oldHole.gameObject);

            return currentHole;
        }
    }
    

}
