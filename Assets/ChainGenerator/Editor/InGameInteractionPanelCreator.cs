using System.Collections;
using System.Collections.Generic;
using ChainEditorHelper;
using ChainInGame;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChainEditor
{
#if UNITY_EDITOR
    public class InGameInteractionPanelCreator : EditorWindow
    {
        [MenuItem("Tools/Chain Generator/InGame Chain Handler")]
        public static void OpenPrefab()
        {
            if (FindObjectOfType<InGameChainHandler>() != null)
            {
                Debug.Log("There's already an InGameChainHandler Prefab on scene");
                return;
            }
            
            var canvas = FindObjectOfType<Canvas>();
            if (!canvas)
            {
                GameObject canvasObject = new GameObject("Canvas");
                canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObject.AddComponent<CanvasScaler>();
                canvasObject.AddComponent<GraphicRaycaster>();
            }
            
            if (FindObjectOfType<EventSystem>() == null)
            {
                GameObject eventSystemObject = new GameObject("EventSystem");
                EventSystem eventSystem = eventSystemObject.AddComponent<EventSystem>();
                
                eventSystemObject.AddComponent<StandaloneInputModule>(); 
                // eventSystemObject.AddComponent<TouchInputModule>(); 
            }
            
            GameObject prefab = PathHelper.FindObjectByGuid("InGameChainHandler"); 
            
            if (prefab != null)
            {
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab, canvas.transform) as GameObject;
            }
            else
            {
                Debug.LogError("InGameChainHandler Prefab not found at the specified path.");
            }
        }
    }
}
#endif