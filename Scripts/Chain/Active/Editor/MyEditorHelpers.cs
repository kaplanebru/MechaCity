using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    public class MyEditorHelpers
    {
        public static string WriteAssetPath(string fileName, string subFolderName)
        {
            string basePath = "Assets/Resources/" + subFolderName +"/";
            return Path.Combine(basePath, fileName + ".asset");
        }

        public static string WritePrefabPath(string fileName, string subFolderName)
        {
            string basePath = "Assets/Resources/" + subFolderName +"/";
            return Path.Combine(basePath, fileName + ".prefab");
        }
        
        public static int GetTypeIndex(string typeName)
        {
            //var allChainDatas = Resources.LoadAll<ChainData>("ChainDatas");
            string[] guids = AssetDatabase.FindAssets("t:"+typeName);
            return guids.Length + 1;
        }

        public static string FindPathByGuid(string objectName)
        {

            // string searchType = "t:Machinery"; //"t:" + typeName; //"t:Machinery"; // Adjust the type based on your asset type
            // string searchString = "t:Machinery Machinery2";//$"{searchType} {objectName}";
            
            string[] guids = AssetDatabase.FindAssets(objectName);
            Debug.Log(guids.Length);
            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            return assetPath;
        }

        public static GameObject FindObjectByGuid(string objectName)
        {
            string assetPath =  FindPathByGuid(objectName);
            
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
