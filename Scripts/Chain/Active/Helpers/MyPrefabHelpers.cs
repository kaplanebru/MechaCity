using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Chain
{
    public class MyPrefabHelpers
    {
        

        public static bool IsPrefabInstance(GameObject gameObject)
        {
            PrefabAssetType assetType = PrefabUtility.GetPrefabAssetType(gameObject);
            PrefabInstanceStatus instanceStatus = PrefabUtility.GetPrefabInstanceStatus(gameObject);

            if (assetType == PrefabAssetType.NotAPrefab)
            {
                //Debug.Log("Not a Prefab");
                return false;
            }

            if (instanceStatus == PrefabInstanceStatus.NotAPrefab)
            {
                Debug.Log("Prefab Asset");
                return false;
            }


            Debug.Log("Prefab Instance");
            return true;
        } //enumla prefab mi instance mi yoksa obsolate mi bakarız

        public static void UnpackPrefabInstance(GameObject gameObject)
        {
            if (IsPrefabInstance(gameObject))
            {
                PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.OutermostRoot,
                    InteractionMode.AutomatedAction);

                if (gameObject.transform.CompareTag("Model"))
                {
                    gameObject.transform.tag = "Untagged";
                    if (!IsPrefabInstance(gameObject))
                    {
                        gameObject.name = "Machinery Copy";
                    }
                }
            }
        }
        
        public static void OverrideChanges(GameObject gameObject)
        {
            Debug.Log("override");
            PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.UserAction);
        }
        
        public static void ApplyChangesToPrefab(GameObject gameObject)
        {
            if (PrefabUtility.IsPartOfPrefabInstance(gameObject))
            {
                GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject) as GameObject;

                if (prefab != null)
                    PrefabUtility.ApplyPrefabInstance(gameObject, InteractionMode.AutomatedAction);
                
                else
                    Debug.LogWarning("Prefab not found.");
            }
            else
                Debug.LogWarning("This GameObject is not a prefab instance.");
        }

    }
}


// void SavePrefab()
// {
//     GameObject machineryPrefab = PrefabUtility.GetCorrespondingObjectFromSource(gameObject);
//     
//     if(machineryPrefab == null) return;
//     
//     //prefab.GetComponent<MyComponent>().myValue = newValue;
//
//
//     PrefabUtility.RecordPrefabInstancePropertyModifications(machineryPrefab);
//     PrefabUtility.SavePrefabAsset(machineryPrefab);
// }

// void UnpackPrefabInstance()
// {
//     if (!Application.isPlaying)
//     {
//         PrefabType prefabType = PrefabUtility.GetPrefabType(gameObject);
//
//
//         if (prefabType == PrefabType.PrefabInstance || prefabType == PrefabType.DisconnectedPrefabInstance)
//         {
//             PrefabUtility.UnpackPrefabInstance(gameObject, PrefabUnpackMode.OutermostRoot,
//                 InteractionMode.AutomatedAction);
//         }
//     }
// }