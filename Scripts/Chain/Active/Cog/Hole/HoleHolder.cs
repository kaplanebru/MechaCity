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
        private Hole _currentHole;
        private Hole _oldHole;

        public Hole CreateHole(int i)
        {
            //if(currentHole != null)
            _oldHole = _currentHole;
            
            if(i < assetHolder.HoleTypes.Count)
                _currentHole = Instantiate(assetHolder.HoleTypes[i], transform);
            
            if(_oldHole != null)
                DestroyImmediate(_oldHole.gameObject);

            return _currentHole;
        }
    }
    
}
