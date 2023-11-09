using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEditor;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class Machinery : MonoBehaviour
    {
        [HideInInspector]public bool isChainRelated = false;
        
        [HideInInspector]public CogHolder cogHolder;
        [HideInInspector]public ChainSpawner chainSpawner;
        [HideInInspector]public ChainDrawer chainDrawer;
        public LinksPool linksPool;
        public ChainAssetHolder assetHolder;


        private void OnEnable()
        {
            cogHolder = GetComponentInChildren<CogHolder>();
            chainSpawner = GetComponentInChildren<ChainSpawner>();
            chainDrawer = GetComponentInChildren<ChainDrawer>();
            linksPool = GetComponentInChildren<LinksPool>();
            
            //ChainEvents.OnCogsReady += UpdateArcs;: cogs hazır olunca cogholdera yollamaya gerek var mı data updatei için?

        }

        private void Start()
        {
            chainSpawner.UpdateArcs(cogHolder.cogs.ToArray());
        }


        private void OnDisable()
        {
            

        }


        //todo: first set cogs from here, later start chain process


       
    }
}


// void SavePrefab()
// {
//     GameObject machineryPrefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
//     
//     if(machineryPrefab == null) return;
//     
//     //prefab.GetComponent<MyComponent>().myValue = newValue;
//
//
//     PrefabUtility.RecordPrefabInstancePropertyModifications(machineryPrefab);
//     PrefabUtility.SavePrefabAsset(machineryPrefab);
// }
