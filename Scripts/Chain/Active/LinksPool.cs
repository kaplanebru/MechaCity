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

        private PlayModeStateChange _state = PlayModeStateChange.EnteredEditMode;
    
        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            if (_state == PlayModeStateChange.EnteredEditMode)
            {
                EnablePool();
            }
          
        }
        
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            _state = state;
            if (_state == PlayModeStateChange.ExitingEditMode)
            {
               print(pool.Count + state);
            }
        }


        void EnablePool()
        {
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

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

        }
    }
}

