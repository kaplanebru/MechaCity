using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Chain;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


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

        private Mover[] _movers;
        private IMachinePart[] _machineParts;

        //public ChainAssetHolder assetHolder;

        private void OnEnable()
        {
            if (!Application.isPlaying)
                GetObjects();
            if (Application.isPlaying)
                SetMovers();
        }
        
        private void Start()
        {
            //if (transform.CompareTag("Model"))
                MyPrefabHelpers.UnpackPrefabInstance(gameObject);
        }
        
        public void To2D()
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        void GetObjects()
        {
            _machineParts = GetComponentsInChildren<IMachinePart>();
            cogHolder = GetComponentInChildren<CogHolder>();
            residual = GetComponentInChildren<Residual>();

            chainGenerator = (ChainGenerator) _machineParts.FirstOrDefault(m => m is ChainGenerator);
            cogHolder.GetCogs(_machineParts.OfType<Cogwheel>());
        }

        void SetMovers()
        {
            ////machineParts.OfType<Mover>().ToArray();
            _machineParts = GetComponentsInChildren<IMachinePart>();
            _movers = GetComponentsInChildren<Mover>();
            
            for (var i = 0; i < _movers.Length; i++)
            {
                var mover = _movers[i];
                mover.MachinerySetup(machinerySpeed, gameObject.GetInstanceID(), _machineParts[i].GetMoverData());

                if (mover is ChainMover chainMover)
                {
                    chainMover.Setup(chainGenerator.links);
                    chainMover.StartCoroutine("StartMover");

                }
                
                // mover.DataSetup(machineParts[i].GetMoverData()); //tek sorun aynı sırayla dizilmiş olup olmamaları: mover ve machinepartın
            }
        }

        public void SaveOnExistingPrefab()
        {
            if (MyPrefabHelpers.IsPrefabInstance(gameObject))
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