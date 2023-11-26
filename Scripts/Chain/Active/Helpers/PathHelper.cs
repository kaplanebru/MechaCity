using System.IO;
using UnityEditor;
using UnityEngine;


namespace Chain
{
    public class PathHelper
    {
         public static string WriteAssetPath(string fileName, string subFolderName)
        {
            string basePath = "Assets/Resources/Chain/" + subFolderName +"/";
            return Path.Combine(basePath, fileName + ".asset");
        }

        public static string WritePrefabPath(string fileName, string subFolderName)
        {
            string basePath = "Assets/Resources/Chain/" + subFolderName +"/";
            return Path.Combine(basePath, fileName + ".prefab");
        }
        
        public static int GetTypeIndex(string typeName)
        {
            //var allChainDatas = Resources.LoadAll<ChainData>("ChainDatas");
            string[] guids = AssetDatabase.FindAssets("t:"+typeName);
            return guids.Length + 1;
        }

        public static string FindPathByName(string objectName)
        {

            // string searchType = "t:Machinery"; //"t:" + typeName; //"t:Machinery"; // Adjust the type based on your asset type
            // string searchString = "t:Machinery Machinery2";//$"{searchType} {objectName}";
            
            string[] guids = AssetDatabase.FindAssets(objectName);
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            return assetPath;
        }

        public static  string[] FindGuidsByType(string typeName)//FindGuidsByType<T>(T type) where T: Component  //
        {
            string[] guids = AssetDatabase.FindAssets("t:" + typeName);
            Debug.Log("t:" + typeName);
            Debug.Log(guids.Length);
            return guids;
        }

        public static GameObject FindObjectByGuid(string objectName)
        {
            string assetPath =  FindPathByName(objectName);
            
            if (!string.IsNullOrEmpty(assetPath))
            {
                Object loadedObject = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                if (loadedObject is GameObject)
                {
                    GameObject foundObject = (GameObject)loadedObject;
                    Debug.Log("Found GameObject: " + foundObject.name);
                    return foundObject;
                }
            }

            return null;
        }
    }
}

