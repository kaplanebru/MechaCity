using UnityEditor;
using UnityEngine;


namespace ChainEditorHelper
{

    public class MyPrefabHelpers
    {
        public static bool IsPrefabInstance(GameObject gameObject)
        {
#if UNITY_EDITOR
            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(gameObject);
            PrefabInstanceStatus instanceStatus = PrefabUtility.GetPrefabInstanceStatus(gameObject);


            if (assetType == PrefabAssetType.NotAPrefab)
            {
                //Debug.Log("Not a Prefab");
                return false;
            }

            if (instanceStatus == PrefabInstanceStatus.NotAPrefab)
            {
                //Debug.Log("Prefab Asset");
                return false;
            }
#endif
            
            //Debug.Log("Prefab Instance");
            return true;
        }

        public static void UnpackPrefabInstance(GameObject gameObject)
        {
            if (IsPrefabInstance(gameObject))
            {
#if UNITY_EDITOR
                PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
#endif
                ChangeNature(gameObject);
            }
        }
        
        public static void ChangeNature(GameObject gameObject)
        {
            if (gameObject.transform.CompareTag("Model"))
            {
                gameObject.transform.tag = "Untagged";
                if (!IsPrefabInstance(gameObject))
                {
                    gameObject.name = "Machinery Instance";
                }
            }
        }
        
        public static void OverrideChanges(GameObject gameObject)
        {
            Debug.Log("override");
#if UNITY_EDITOR
            PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.UserAction);
#endif
        }
        
        // public static void ApplyChangesToPrefab(GameObject gameObject)
        // {
        //     if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
        //     {
        //         GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject) as GameObject;
        //
        //         if (prefab != null)
        //             PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.AutomatedAction);
        //         
        //         else
        //             Debug.LogWarning("Prefab not found.");
        //     }
        //     else
        //         Debug.LogWarning("This GameObject is not a prefab instance.");
        // }

    }
}


