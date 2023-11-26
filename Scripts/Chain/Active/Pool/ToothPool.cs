using Chain;
using GenericHelper;
using UnityEngine;

[ExecuteInEditMode]
//[InitializeOnLoad]

public class ToothPool : Pool<Tooth>
{
    
    // [SerializeField] int population = 1000;
    // [SerializeField] Tooth toothPrefab;
    //
    //
    // private void OnEnable()
    // {
    //     if (Instance == null) 
    //         Instance = this;
    //    // ChainEvents.OnCogDataSet += ReadyForPool;
    //
    //     EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    //     
    // }
    //
    // private void ReadyForPool(CogData arg1, Transform arg2)
    // {
    //     if (Application.isEditor)
    //     {
    //         if (Instance == null) 
    //             Instance = this;
    //     
    //         if (transform.childCount == 0)
    //         {
    //             CreatePool(population, transform, toothPrefab);
    //         }
    //         else
    //         {
    //             var oldTeeth = GetComponentsInChildren<Tooth>(true);
    //             RestorePool(oldTeeth);
    //         }
    //         
    //         print(pool.Count);
    //     }
    //    
    //     ChainEvents.OnPoolReady?.Invoke(arg1, arg2);
    //    
    // }
    //
    // private void OnPlayModeStateChanged(PlayModeStateChange state)
    // {
    //     // if(state == PlayModeStateChange.ExitingEditMode)
    //     //     print("exiting edit mode");
    //     // else if (state == PlayModeStateChange.EnteredEditMode)
    // }
    //
    // private void OnDisable()
    // {
    //    // ChainEvents.OnCogDataSet -= ReadyForPool;
    //     EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    // }
}
