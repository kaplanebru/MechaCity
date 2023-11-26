using System.Collections;
using System.Collections.Generic;
using GenericHelper;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    public abstract class PoolCreator<T, U> : EditorWindow where T : Pool<U> where U : Component
    {
        [SerializeField] protected int amount = 100;
        [SerializeField] protected U objectPrefab;
        [SerializeField] protected string poolName;
        protected T _pool;
        
        
        private void OnGUI()
        {
            GUILayout.Label(typeof(T).Name + " Pool Creator", EditorStyles.boldLabel);
            
            EditorGUI.BeginChangeCheck();

            poolName = EditorGUILayout.TextField("Pool Name", poolName); //write the same name if you want to modify pool + reset pool before
            amount = EditorGUILayout.IntField("Amount", amount);
            objectPrefab = (U) EditorGUILayout.ObjectField("Object Prefab", objectPrefab, typeof(U), false);
            
            if (GUILayout.Button("Create Pool"))
            {
                CreatePoolPrefab();
            }
            
            EditorGUI.EndChangeCheck();
        }

        void CreatePoolPrefab()
        {
            GameObject go = new GameObject(poolName);

            go.AddComponent<T>();
            _pool = go.GetComponent<T>();
            InitializePool();

            
            PrefabUtility.SaveAsPrefabAsset(go,  PathHelper.WritePrefabPath(poolName, typeof(T).Name));

            DestroyImmediate(go);
        }
        
        void InitializePool()
        {
            _pool.CreatePool(amount, _pool.transform, objectPrefab);
        }

   
    }
}

