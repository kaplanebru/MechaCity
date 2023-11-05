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
        [SerializeField] int population = 100;
        [SerializeField] ChainLink linkPrefab;
        public void ActivatePool()
        {
            if (pool.Count == 0)
            {
                RestorePool(GetComponentsInChildren<ChainLink>(true));
                if(pool.Count == 0)
                    CreatePool(population, transform, linkPrefab);
            }
        
            print("pool count " +pool.Count);
        }

        

        public void DeleteLinks()
        {
            ChainEvents.OnDeleteLinks?.Invoke();

            var links = GetComponentsInChildren<ChainLink>(true);
       
            for (int i = links.Length - 1; i >= 0; i--)
            {
                var link = links[i];
                DestroyImmediate(link.gameObject, true);
            }
            
            pool.Clear();
        }
        
    }
}

