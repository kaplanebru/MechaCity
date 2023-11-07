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
        [SerializeField] private string poolName;
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

            poolName = EditorGUILayout.TextField("Pool Name", poolName); //write the same name if you want to modify pool + reset pool before
            amount = EditorGUILayout.IntField("Amount", amount);
            chainLinkPrefab = (ChainLink) EditorGUILayout.ObjectField("Link Prefab", chainLinkPrefab, typeof(ChainLink), false);
            
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

            var path = MyEditorHelpers.GetPath(poolName, "LinkPools");
            
            PrefabUtility.SaveAsPrefabAsset(go, path);//"Assets/LinksPool.prefab"); //EditorHelpers.GetPath(nameof(go) + newIndex, "")

            DestroyImmediate(go);
        }
        
        void InitializePool()
        {
            _linksPool.CreatePool(amount, _linksPool.transform, chainLinkPrefab);
        }
    }
}