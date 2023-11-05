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
        //[SerializeField] int population = 0;
        //[SerializeField] ChainLink linkPrefab;
        
        
        public void ActivatePool(int pointCount, ChainLink linkPrefab)
        {
            if (pool.Count == 0)
            {
                var poolChildren = GetComponentsInChildren<ChainLink>(true);
                var childrenLength = poolChildren.Length;
                
                RestorePool(poolChildren);
                if (pointCount > childrenLength)
                {
                    CreatePool(pointCount-childrenLength, transform, linkPrefab);
                }
                
                if(pool.Count == 0 && childrenLength == 0)  //not: eşitse de 0 olur
                    CreatePool(pointCount, transform, linkPrefab);
            }
        }

        

        public void DeleteLinks()
        {

            var links = GetComponentsInChildren<ChainLink>(true);
            
            for (int i = links.Length - 1; i >= 0; i--)
            {
                var link = links[i];
                DestroyImmediate(link.gameObject, true);
            }
            
            pool.Clear();
            ChainEvents.OnDeleteLinks?.Invoke();
        }
        
        
    }
}

