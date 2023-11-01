using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenericHelper;
using UnityEngine;

namespace Chain
{
    [ExecuteInEditMode]
    public class LinksPool : Pool<ChainLink>
    {
        [SerializeField] int population = 100;
        [SerializeField] ChainLink linkPrefab;
    
        private void OnEnable()
        {
            // if(transform.childCount == 0)
            //     CreatePool(population, transform, linkPrefab);
            // else
            //     RestorePool(GetComponentsInChildren<ChainLink>(true)); //TODO: kendisi de ekli, link diye class açmak lazım

            if (pool.Count == 0)
            {
                RestorePool(GetComponentsInChildren<ChainLink>(true)); //TODO: kendisi de ekli, link diye class açmak lazım
                if(pool.Count == 0)
                    CreatePool(population, transform, linkPrefab);
            }
        
            print(pool.Count);
            print("pool enabled");
        }
    
    

        public void DeleteLinks()
        {
            var links = GetComponentsInChildren<ChainLink>();
       
            for (int i = links.Length - 1; i >= 0; i--)
            {
                var link = links[i];
                DestroyImmediate(link.gameObject, true);
            }
        }
    }
}

