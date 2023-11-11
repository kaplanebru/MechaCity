using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericHelper;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class TeethPool : Pool<Tooth>
    {
        public Tooth toothPrefab;
        private void OnEnable()
        {
            //CreatePool(200, transform, toothPrefab);
        }

        public void ActivatePool(int pointCount, Tooth toothPrefab)
        {
            if (pool.Count > 0) return;
            
            var poolChildren = GetComponentsInChildren<Tooth>(true).ToList();
            var childrenLength = poolChildren.Count;

            if (childrenLength > 0)
                RestorePool(poolChildren.ToArray());
            
        }
        
        public void DeleteLinks()
        {
            ChainEvents.OnDeleteTeeth?.Invoke();
            DestroyImmediate(gameObject, true);
        }
    }

}
