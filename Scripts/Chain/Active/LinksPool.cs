using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericHelper;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    [ExecuteInEditMode]
    public class LinksPool : Pool<ChainLink>
    {
        

        public void ActivatePool(int pointCount, ChainLink linkPrefab)
        {
            if (pool.Count > 0) return;
            
            var poolChildren = GetComponentsInChildren<ChainLink>(true).ToList();
            var childrenLength = poolChildren.Count;

            if (childrenLength > 0)
                RestorePool(poolChildren.ToArray());
            
        }
        

        public void DeleteLinks()
        {
            ChainEvents.OnDeleteLinks?.Invoke();
            DestroyImmediate(gameObject, true);
            
        }
    }
}