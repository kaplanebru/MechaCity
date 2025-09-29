using Chain;
using ChainEditorHelper;
using UnityEditor;
using UnityEngine;

namespace ChainEditor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(HoleCreator))]
    public class CustomHoleCreatorEditor : Editor
    {
        public GameObject holeModelPrefab;
        public string holeName;

        private HoleCreator _holeCreator;
        private GameObject _holeModel;
        private Hole _hole;
        private string[] _messages = new string[3];

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
            holeModelPrefab =
                (GameObject) EditorGUILayout.ObjectField("Hole Model", holeModelPrefab, typeof(GameObject), false);
            if (GUILayout.Button("Create Hole"))
            {
                if (holeModelPrefab == null)
                {
                    Debug.LogWarning("Hole Model is Null");
                    return;
                }

                CreateHole();
            }

            EditorGUI.EndChangeCheck();
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            WarningMessage();
            EditorGUILayout.Space();
        }

        void CreateHole()
        {
            GameObject go = new GameObject(holeName);
            go.AddComponent<Hole>();
            _hole = go.GetComponent<Hole>();
            _hole.Id = _holeCreator.holeAssetHolder.HoleTypes.Count;

            CreateHoleModel();

            var holePrefab = PrefabUtility.SaveAsPrefabAsset(go, PathHelper.WriteAssetPath(holeName, "Holes", true));
            _holeCreator.holeAssetHolder.AddHole(holePrefab.GetComponent<Hole>());
            DestroyImmediate(go);
        }

        void CreateHoleModel()
        {
            _holeModel = Instantiate(holeModelPrefab, _hole.transform);
            _holeModel.transform.GetChild(0).GetComponentInChildren<MeshRenderer>().material =
                _holeCreator.holeAssetHolder.HoleMaterial; //_holeMat;
            //_holeModel.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }

        void WarningMessage()
        {
            _messages[0] = "Warning: The hole model must include two parts!"; // \n  \n 
            _messages[1] = "    The first part for the hole surface";
            _messages[2] = "    The second part for the edges with flipped normals.";

            EditorGUILayout.LabelField( _messages[0], EditorStyles.boldLabel);
            EditorGUILayout.LabelField( _messages[1]);
            EditorGUILayout.LabelField( _messages[2]);
            
            //EditorUtility.DisplayDialog("Caution", message1, "OK");
        }
        
        
    }
}
#endif