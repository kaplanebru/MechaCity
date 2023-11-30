using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    [CustomEditor(typeof(HoleCreator))]
    public class CustomHoleCreatorEditor : Editor
    {
        
        private HoleCreator _holeCreator;
        public GameObject holeModel;
        public string holeName;
        private GameObject[] _holeChildren = new GameObject[2];
        private Hole _hole;

        [SerializeField] Material _holeMat;
    

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isPlaying) return;
            DrawDefaultInspector();

            if (_holeCreator == null)
                _holeCreator = target as HoleCreator;
            
            EditorGUILayout.Space();

            GUILayout.Label("CUSTOM HOLE CREATOR", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            holeName = EditorGUILayout.TextField("Hole Name", holeName);
            holeModel = (GameObject) EditorGUILayout.ObjectField("Hole Model", holeModel, typeof(GameObject), false);
            if (GUILayout.Button("Create Hole"))
            {
                if (holeModel == null)
                {
                    Debug.LogWarning("Hole Model is Null");
                    return;
                }

                CreateHole();
            }

            EditorGUI.EndChangeCheck();
        }

        void CreateHole()
        {
            GameObject go = new GameObject(holeName);
            go.AddComponent<Hole>();
            _hole = go.GetComponent<Hole>();

            SetHoleType();
            //GetHoleMaterial();
            AddModels();

            PrefabUtility.SaveAsPrefabAsset(go, PathHelper.WritePrefabPath(holeName, "Holes"));
            DestroyImmediate(go);
        }

        void AddModels()
        {
            for (int i = 0; i < _holeChildren.Length; i++)
            {
                _holeChildren[i] = Instantiate(holeModel, _hole.transform);
                _holeChildren[i].GetComponentInChildren<MeshRenderer>().material = _holeCreator.holeAssetHolder.HoleMaterial; //_holeMat;
                _holeChildren[i].transform.localRotation = Quaternion.Euler(-90, 0, 0);
            }
        }

        void GetHoleMaterial()
        {
            //_holeMat = Resources.Load<Material>("Chain/Holes/Material/HoleMat");
        }

        void SetHoleType()
        {
            //_hole.holeType = ChainEnums.HoleType.Custom;
           
            _holeCreator.holeAssetHolder.HoleTypes.Add(_hole);
            _hole.Id = _holeCreator.holeAssetHolder.HoleTypes.Count;
            _holeCreator.holeAssetHolder.RestoreHoleLabels();
            _holeCreator.holeAssetHolder.holeHolder.ResetHoles();
        }
    }
}