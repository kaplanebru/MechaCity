using System.Collections;
using System.Collections.Generic;
using MyNamespace;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    public class LinkPoolCreator : EditorWindow
    {
        [SerializeField] private int amount = 100;
        [SerializeField] private ChainLink chainLinkPrefab;
        [SerializeField] private ChainEnums.PoolFunction poolFunction;
        private LinksPool _linksPool;
        
        [MenuItem("Tools/Link Pool Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LinkPoolCreator));
        }
    
        private void OnGUI()
        {
            GUILayout.Label("Link Pool Creator", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            poolFunction = (ChainEnums.PoolFunction) EditorGUILayout.EnumPopup("Pool Function", poolFunction);
            amount = EditorGUILayout.IntField("Amount", amount);
            chainLinkPrefab = (ChainLink) EditorGUILayout.ObjectField("Link Prefab", chainLinkPrefab, typeof(ChainLink), false);

            switch (poolFunction)
            {
                case ChainEnums.PoolFunction.CreateNewPool:
                {
                    if (GUILayout.Button("Create Pool"))
                    {
                        CreatePoolPrefab();
                    }

                    break;
                }
                case ChainEnums.PoolFunction.ModifyPool:
                {
                    _linksPool = (LinksPool) EditorGUILayout.ObjectField("Pool Prefab", _linksPool, typeof(LinksPool), false);
                    if (_linksPool != null)
                    {
                        if (GUILayout.Button("Create Pool"))
                        {
                           // ModifyPoolPrefab();
                        }
                       
                    }
                    break;
                }
                    
            }
           

            EditorGUI.EndChangeCheck();

        }

        void CreatePoolPrefab()
        {
            GameObject go = new GameObject("LinksPool");
      
            go.AddComponent<LinksPool>();
            _linksPool = go.GetComponent<LinksPool>();
            InitializePool();
            PrefabUtility.SaveAsPrefabAsset(go, "Assets/LinksPool.prefab"); //EditorHelpers.GetPath(nameof(go) + newIndex, "")


            DestroyImmediate(go);
        }

        // void ModifyPoolPrefab()
        // {
        //     _linksPool.DeleteLinks();
        //     InitializePool();
        //     
        //     Debug.Log(_linksPool.pool.Count);
        //
        //     
        //     PrefabUtility.SaveAsPrefabAsset(_linksPool.gameObject, "Assets/LinksPool.prefab"); //todo: FİND PATH
        //     //PrefabUtility.SavePrefabAsset()
        // }

        void InitializePool()
        {
            _linksPool.CreatePool(amount, _linksPool.transform, chainLinkPrefab);
        }
    }
}

