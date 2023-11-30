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
        

        public void ResetHoles()
        {
            foreach (var holeAsset in assetHolder.HoleTypes)
            {
                Instantiate(holeAsset, transform);
            }
            MyPrefabHelpers.ApplyChangesToPrefab(gameObject);
        }

        public void CreateHole(int i)
        {
            var oldHole = currentHole.gameObject;
            currentHole = Instantiate(assetHolder.HoleTypes[i], transform);
            DestroyImmediate(oldHole);
        }
    }
    

}
