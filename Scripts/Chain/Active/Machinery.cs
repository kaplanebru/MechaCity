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
        [HideInInspector]public bool isChainRelated = false;
        public float machinerySpeed;
        [HideInInspector]public ChainData ChainData;
        [HideInInspector]public LinksPool linksPool;
        public List<ChainLink> links = new();


        [HideInInspector]public CogHolder cogHolder;
        [HideInInspector]public Residual residual;
        [HideInInspector]public Mover[] movers;
       
        public ChainAssetHolder assetHolder;

        
        private void Start()
        {
            MyPrefabHelpers.UnpackPrefabInstance(gameObject);
        }
       

        public void To2D()
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        
        // ChainEvents.OnDeleteLinks -= DeletePoolClearLinks;


        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                if(isChainRelated)
                    StartPool();
                cogHolder = GetComponentInChildren<CogHolder>();
                residual = GetComponentInChildren<Residual>();
            }
           
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
            if (linksPool != null) return;
            linksPool = GetComponentInChildren<LinksPool>();
            if (linksPool == null) linksPool = CreatePool();
        }

        public LinksPool CreatePool()
        {
            linksPool = Instantiate(ChainData.LinksPoolPrefab, transform);
            return linksPool;
        }

        public void DeletePoolClearLinks() //on delete links
        {
            links.Clear();
            linksPool.DeletePool();
        }
        
        public void SaveOnExistingPrefab()
        {
            GameObject newInstance = Instantiate(gameObject);
            PrefabUtility.SaveAsPrefabAsset(newInstance,
                MyEditorHelpers.FindPathByGuid(name));

            DestroyImmediate(newInstance);
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

        private void OnDisable()
        {
        }


        //todo: first set cogs from here, later start chain process
    }
}

