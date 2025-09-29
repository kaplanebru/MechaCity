using Chain;
using ChainEditorHelper;
using UnityEditor;
using UnityEngine;

namespace ChainEditor
{
#if UNITY_EDITOR
    public class ChainAndCogOpener : EditorWindow
    {
        [MenuItem("Tools/Chain Generator/Create Chain and Gear System")]
        public static void OpenPrefab()
        {
            GameObject prefab = PathHelper.FindObjectByGuid("_MachineryModel");

            if (prefab != null)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

                Selection.activeGameObject = instance;
                MyPrefabHelpers.ChangeNature(instance.gameObject);
                instance.name = "Machinery Instance";
            }
            else
            {
                Debug.LogError("Machinery Prefab not found at the specified path.");
            }
        }
    }
}
#endif
