using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    public class CustomHoleCreator : EditorWindow
    {
        public GameObject holeModel;
        public string holeName;
        private GameObject[] _holeChildren = new GameObject[2];
        private Hole _hole;
        private Material _holeMat;
        [MenuItem("Tools/Chain Generator/Custom Hole Creator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(CustomHoleCreator));
        }

        private void OnGUI()
        {
            GUILayout.Label("Custom Hole Creator", EditorStyles.boldLabel);
            
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
            GetHoleMaterial();
            AddModels();
            
            PrefabUtility.SaveAsPrefabAsset(go,  PathHelper.WritePrefabPath(holeName, "Holes"));
            DestroyImmediate(go);
        }

        void AddModels()
        {
            for (int i = 0; i < _holeChildren.Length; i++)
            {
                _holeChildren[i] = Instantiate(holeModel, _hole.transform);
                _holeChildren[i].GetComponentInChildren<MeshRenderer>().material = _holeMat;
                _holeChildren[i].transform.localRotation = Quaternion.Euler(-90, 0, 0);
            }
        }

        void GetHoleMaterial()
        {
            _holeMat =  Resources.Load<Material>("Chain/Holes/Material/HoleMat");
               // PathHelper.WriteAssetPath("HoleMat", "Holes/Material")//PathHelper.FindObjectByGuid("HoleMat").GetComponent<Material>();
        }

        void SetHoleType()
        {
            _hole.holeType = ChainEnums.HoleType.Custom;
        }
    }

}
