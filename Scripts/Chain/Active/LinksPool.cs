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
        // [SerializeField] int population = 100;
        // [SerializeField] ChainLink linkPrefab;
        //[SerializeField] private List<ChainLink> poolChildren = new();
        [SerializeField] 


        public void ActivatePool(int pointCount, ChainLink linkPrefab)
        {
            print("activate pool");

            if (pool.Count > 0) return;

            print("pool: " + pool.Count);

            var poolChildren = GetComponentsInChildren<ChainLink>(true).ToList();
            var childrenLength = poolChildren.Count;

            if (childrenLength > 0)
                RestorePool(poolChildren.ToArray());


            // if (childrenLength == 0)
            // {
            //     if(pool.Count > 0) return;
            //     print("children " +childrenLength);
            //
            //     CreatePool(pointCount, transform, linkPrefab);
            // }
            // else
            // {
            //     RestorePool(poolChildren.ToArray());
            //     if (pointCount > childrenLength)
            //     {
            //         CreatePool(pointCount-childrenLength, transform, linkPrefab);
            //     }
            // }
        }

        // public void InitializePool()
        // {
        //     CreatePool(population, transform, linkPrefab);
        // }


        public void DeleteLinks()
        {
            // ChainEvents.OnDeleteLinks?.Invoke();
            // DestroyImmediate(gameObject, true);

            var links = GetComponentsInChildren<ChainLink>(true);
            
            for (int i = links.Length - 1; i >= 0; i--)
            {
                var link = links[i];
                DestroyImmediate(link.gameObject, true);
                
            }
            
            pool.Clear();
            
            //transform.DetachChildren();
            // ChainEvents.OnDeleteLinks?.Invoke();
            //
            // print("after delete children: " + GetComponentsInChildren<ChainLink>(true).Length);
        }
    }
}