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
    }

}
