using System.IO;
using UnityEditor;
using UnityEngine;



namespace ChainEditorHelper
{

    public class PathHelper
    {
        public static string GetFolder(string folderName)
        {
#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets("t:Folder " + folderName);

            if (guids.Length > 0)
            {
                string folderPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                return folderPath;
            }
#endif

            Debug.Log("Folder not found: " + folderName);
            return null;
        }

        public static string WriteAssetPath(string fileName, string folderName, bool isPrefab = false)
        {
            string subFolder = isPrefab ? "/Prefabs/" : "/Data/";
            string basePath = GetFolder("ChainGenerator") + subFolder + folderName + "/";
            
            string type = isPrefab ? ".prefab" : ".asset";
            return Path.Combine(basePath, fileName + type);
        }

        // public static int GetTypeIndex(string typeName)
        // {
        //     //var allChainDatas = Resources.LoadAll<ChainData>("ChainDatas");
        //     string[] guids = AssetDatabase.FindAssets("t:" + typeName);
        //     return guids.Length + 1;
        // }

        public static string FindPathByName(string objectName)
        {
            // string searchType = "t:Machinery"; //"t:" + typeName; //"t:Machinery"; // Adjust the type based on your asset type
            // string searchString = "t:Machinery Machinery2";//$"{searchType} {objectName}";

#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets(objectName);
            if (guids.Length == 0)
            {
                Debug.LogWarning("There is no such prefab");
                return null;
            }
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            return assetPath;
#else
    return null;
#endif
        }

        // public static string[] FindGuidsByType(string typeName) //FindGuidsByType<T>(T type) where T: Component  //
        // {
        //     string[] guids = AssetDatabase.FindAssets("t:" + typeName);
        //     Debug.Log("t:" + typeName);
        //     Debug.Log(guids.Length);
        //     return guids;
        // }

        public static GameObject FindObjectByGuid(string objectName)
        {
#if UNITY_EDITOR
            string assetPath = FindPathByName(objectName);

            if (!string.IsNullOrEmpty(assetPath))
            {
                Object loadedObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                if (loadedObject is GameObject)
                {
                    GameObject foundObject = (GameObject) loadedObject;
                    //Debug.Log("Found GameObject: " + foundObject.name);
                    return foundObject;
                }
            }
#endif
            return null;
        }
    }
}
