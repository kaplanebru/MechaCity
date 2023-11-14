using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEditor;
using UnityEngine;


namespace Chain
{
    [ExecuteAlways]
    public class Machinery : MonoBehaviour
    {
        public bool isChainRelated = false;


        [HideInInspector]public CogHolder cogHolder;
        [HideInInspector]public ChainSpawner chainSpawner;
        [HideInInspector]public ChainDrawer chainDrawer;
        [HideInInspector]public Residual residual;
        public LinksPool linksPool;
        public ChainAssetHolder assetHolder;

        public string instanceID; //for debug
        public static string InstanceID { get; private set; }
        

        void SetID()
        {
            InstanceID = Guid.NewGuid().ToString();
            instanceID = InstanceID;
        }

        public void To2D()
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        


        private void OnEnable()
        {
            cogHolder = GetComponentInChildren<CogHolder>();
            chainSpawner = GetComponentInChildren<ChainSpawner>();
            chainDrawer = GetComponentInChildren<ChainDrawer>();
            linksPool = linksPool == null ? CreateLinkPool() : GetComponentInChildren<LinksPool>();
            residual = GetComponentInChildren<Residual>();
            //ChainEvents.OnCogsReady += UpdateArcs;: cogs hazır olunca cogholdera yollamaya gerek var mı data updatei için?
        }

        public void ApplyChangesToPrefab()
        {
            // Check if the object is a prefab instance
            if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
            {
                // Get the prefab asset
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject) as GameObject;

                if (prefab != null)
                {
                    // Apply changes to the prefab
                    PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.AutomatedAction);
                }
                else
                {
                    Debug.LogWarning("Prefab not found.");
                }
            }
            else
            {
                Debug.LogWarning("This GameObject is not a prefab instance.");
            }
        }

        public bool IsPrefabInstance()
        {
            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(gameObject);
            PrefabInstanceStatus instanceStatus = PrefabUtility.GetPrefabInstanceStatus(gameObject);

            if (assetType == PrefabAssetType.NotAPrefab)
            {
                //Debug.Log("Not a Prefab");
                return false;
            }

            if (instanceStatus == PrefabInstanceStatus.NotAPrefab)
            {
                Debug.Log("Prefab Asset");
                return false;
            }


            Debug.Log("Prefab Instance");
            return true;
        } //enumla prefab mi instance mi yoksa obsolate mi bakarız

        void UnpackPrefabInstance()
        {
            if (IsPrefabInstance())
            {
                PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.OutermostRoot,
                    InteractionMode.AutomatedAction);
                SetID();
            }
        }
        
        public LinksPool CreateLinkPool()
        {
            linksPool = Instantiate(chainSpawner.Data.LinksPoolPrefab, transform);
            chainDrawer.GetLinksPool(linksPool);
            return linksPool;
        }


        private void Start()
        {
            UnpackPrefabInstance();
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

// void UnpackPrefabInstance()
// {
//     if (!Application.isPlaying)
//     {
//         PrefabType prefabType = PrefabUtility.GetPrefabType(gameObject);
//
//
//         if (prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance)
//         {
//             PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.OutermostRoot,
//                 InteractionMode.AutomatedAction);
//         }
//     }
// }