using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;


namespace Chain
{
    public class LinkPoolCreator : EditorWindow
    {
        [SerializeField] private int amount = 100;
        [SerializeField] private ChainLink chainLinkPrefab;
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

            amount = EditorGUILayout.IntField("Amount", amount);
            chainLinkPrefab =
                (ChainLink) EditorGUILayout.ObjectField("Link Prefab", chainLinkPrefab, typeof(ChainLink), false);
            if (GUILayout.Button("Create Pool"))
            {
                CreatePoolPrefab();
           
            }

            EditorGUI.EndChangeCheck();

        }

        void CreatePoolPrefab()
        {
            GameObject go = new GameObject("LinksPool");
      
            go.AddComponent<LinksPool>();
            _linksPool = go.GetComponent<LinksPool>();
            InitializePool();
            PrefabUtility.SaveAsPrefabAsset(go, "Assets/LinksPool.prefab");


            DestroyImmediate(go);
        }

        void InitializePool()
        {
            _linksPool.CreatePool(amount, _linksPool.transform, chainLinkPrefab);
        }
    }
}

