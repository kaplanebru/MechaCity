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

        public static string FindPathByGuid(string objectName)
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
        
        public static void DrawFrames(Color frameColor, Rect boxRect)
        {
            EditorGUI.DrawRect(new Rect(boxRect.x, boxRect.y, boxRect.width, 1), frameColor);

            EditorGUI.DrawRect(new Rect(boxRect.x, boxRect.yMax - 1, boxRect.width, 1), frameColor);

            EditorGUI.DrawRect(new Rect(boxRect.x, boxRect.y, 1, boxRect.height), frameColor);

            EditorGUI.DrawRect(new Rect(boxRect.xMax - 1, boxRect.y, 1, boxRect.height), frameColor);
        }
        
        public static void DrawSeparatorLine(Color lineColor)
        {
            Rect lineRect = EditorGUILayout.GetControlRect(false, 2);
            lineRect.height = 1;

            Color originalBackgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = lineColor;

            EditorGUI.DrawRect(lineRect, lineColor);
            GUI.backgroundColor = originalBackgroundColor;
        }
    }

}
