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
            if(transform.CompareTag("Model"))
                MyPrefabHelpers.UnpackPrefabInstance(gameObject);
        }
        
        public void To2D()
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        
        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
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

        public void SaveOnExistingPrefab()
        {
            if(MyPrefabHelpers.IsPrefabInstance(gameObject))
                MyPrefabHelpers.OverrideChanges(gameObject);
            else
            {
                GameObject newInstance = Instantiate(gameObject);
                PrefabUtility.SaveAsPrefabAsset(newInstance,
                    MyEditorHelpers.FindPathByGuid(name));

                DestroyImmediate(newInstance);
                SaveMachinery();
            }
           
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

    }
}