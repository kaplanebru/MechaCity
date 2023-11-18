using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chain;
using UnityEditor;
using UnityEngine;


namespace Chain
{
    [ExecuteAlways]
    public class Machinery : MonoBehaviour
    {
        public bool isChainRelated = false;
        public float machinerySpeed;
        public ChainData ChainData;
        [HideInInspector]public LinksPool linksPool;
        public List<ChainLink> links = new();


        [HideInInspector]public CogHolder cogHolder;
        [HideInInspector]public Residual residual;
        [HideInInspector]public Mover[] movers;
       
        public ChainAssetHolder assetHolder;

        
        private void Start()
        {
            UnpackPrefabInstance();
        }
       

        public void To2D()
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        
        // ChainEvents.OnDeleteLinks -= DeletePoolClearLinks;


        private void OnEnable()
        {
            //if(!Application.isPlaying)
            linksPool = GetComponentInChildren<LinksPool>();
            PoolNull();
            linksPool.ActivatePool();
            cogHolder = GetComponentInChildren<CogHolder>();
            residual = GetComponentInChildren<Residual>();
            movers = GetComponentsInChildren<Mover>();
            
            if (Application.isPlaying)
            {
                print(machinerySpeed);
                foreach (var mover in movers)
                {
                    mover.MachinerySpeed = machinerySpeed;
                }
            }
        }
        
        public void GenerateChain(Action saveCogs)
        {
            var chainRelatedCogs = cogHolder.GetChainRelatedCogs();
            
            if (chainRelatedCogs.Length < 2)
            {
                ResetLinks();
                return;
            }

            foreach (var cog in chainRelatedCogs)
            {
                cog.Data.IsMoving = ChainData.IsMoving;
            }
            saveCogs(); //GenerateCogs();SetDirty

            ChainData.CogAmount = chainRelatedCogs.Length;

            if(PoolNull()) return;
            ResetLinks();
            
            var chainGeneratorData = new ChainGeneratorData(ChainData, linksPool);
            links = new ChainPointCreator(cogHolder.GetChainRelatedCogs(), chainGeneratorData).ExecutePhase();
        }
        
        public void ResetLinks() //direkt pooldan yapılabilir
        {
            links.ForEach(l => linksPool.ReleaseItem(l));
            links.Clear();
        }
        
        void StartPool()
        {
            if (linksPool != null)
                return;
            Debug.Log("pool null");
            linksPool = GetComponentInChildren<LinksPool>();
            if (linksPool == null) linksPool = CreatePool();
        }

        LinksPool CreatePool()
        {
            linksPool = Instantiate(ChainData.LinksPoolPrefab, transform);
            return linksPool;
        }

        public void DeletePoolClearLinks() //on delete links
        {
            links.Clear();
            linksPool.DeletePool();
        }

        bool PoolNull()
        {
            if (linksPool == null)
            {
                linksPool = GetComponentInChildren<LinksPool>();
                if (linksPool == null) //for bug check, temporary
                {
                    Debug.LogError("links pool null");
                    return true;
                }
            }

            if (linksPool.pool.Count == 0)
                linksPool.ActivatePool();
            

            if (links.Count > 0 && links.Any(l => l == null))
            {
                links.Clear();
                Debug.LogError("links null");
            }
                
            return false;
        }
        
        public void ApplyChangesToPrefab()
        {
            if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
            {
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject) as GameObject;

                if (prefab != null)
                    PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.AutomatedAction);
                
                else
                    Debug.LogWarning("Prefab not found.");
            }
            else
                Debug.LogWarning("This GameObject is not a prefab instance.");
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

                if (transform.CompareTag("Model"))
                {
                    transform.tag = "Untagged";
                    if (!IsPrefabInstance())
                    {
                        gameObject.name = "Machinery Copy";
                    }
                }
            }
        }
        
        public void SaveOnExistingPrefab()
        {
            GameObject newInstance = Instantiate(gameObject);
            PrefabUtility.SaveAsPrefabAsset(newInstance,
                MyEditorHelpers.FindPathByGuid(name));

            DestroyImmediate(newInstance);
        }

        public void OverrideChanges()
        {
            Debug.Log("override");
            PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.UserAction);
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