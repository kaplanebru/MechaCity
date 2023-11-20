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
        [HideInInspector] public bool isChainRelated = false;
        [HideInInspector] public float machinerySpeed;


        [HideInInspector] public ChainGenerator chainGenerator;
        [HideInInspector] public CogHolder cogHolder;
        [HideInInspector] public Residual residual;
        [HideInInspector] public Mover[] movers;

        //public ChainAssetHolder assetHolder;


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
                // if(isChainRelated)
                //     StartPool();
                cogHolder = GetComponentInChildren<CogHolder>();
                chainGenerator = GetComponentInChildren<ChainGenerator>();
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


        // void StartPool()
        // {
        //     if (linksPool != null) return;
        //     linksPool = GetComponentInChildren<LinksPool>();
        //     if (linksPool == null) linksPool = CreatePool();
        // }
        //
        // public LinksPool CreatePool()
        // {
        //     linksPool = Instantiate(ChainData.LinksPoolPrefab, transform);
        //     return linksPool;
        // }
        //
        // public void DeletePoolClearLinks() //on delete links
        // {
        //     links.Clear();
        //     linksPool.DeletePool();
        // }


        public void SaveOnExistingPrefab()
        {
            GameObject newInstance = Instantiate(gameObject);
            PrefabUtility.SaveAsPrefabAsset(newInstance,
                MyEditorHelpers.FindPathByGuid(name));

            DestroyImmediate(newInstance);
            SaveMachinery();
        }

        public void SaveMachinery() //todo: buralar machinerye taşınabilir
        {
            Debug.Log("saved");

            EditorUtility.SetDirty(chainGenerator.ChainData); //TODO: sonradan eklendi
            EditorUtility.SetDirty(gameObject); //

            if (MyPrefabHelpers.IsPrefabInstance(gameObject))
                MyPrefabHelpers.OverrideChanges(gameObject);
            else
                residual.CleanResiduals();
        }


        // bool PoolNull()
        // {
        //     if (linksPool == null)
        //     {
        //         linksPool = GetComponentInChildren<LinksPool>();
        //         if (linksPool == null) //for bug check, temporary
        //         {
        //             Debug.LogError("links pool null");
        //             return true;
        //         }
        //     }
        //
        //     if (linksPool.pool.Count == 0)
        //         linksPool.ActivatePool();
        //     
        //
        //     if (links.Count > 0 && links.Any(l => l == null))
        //     {
        //         links.Clear();
        //         Debug.LogError("links null");
        //     }
        //         
        //     return false;
        // }


        private void OnDisable()
        {
        }


        //todo: first set cogs from here, later start chain process
    }
}