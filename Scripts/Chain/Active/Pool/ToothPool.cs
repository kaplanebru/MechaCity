using System;
using System.Linq;
using Chain;
using GenericHelper;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class ToothPool : Pool<Tooth>
{

    [SerializeField] int population = 1000;
    [SerializeField] Tooth toothPrefab;

  
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            ChainEvents.OnPoolCreated?.Invoke();
        }

        print("pool awaken");
        
    }

    
    private void OnEnable()
    {
        print("pool enbaled");
        if (instance == null)
        {
            instance = this;
            ChainEvents.OnPoolCreated?.Invoke();
        }
        
        if (!Application.isPlaying)
        {
            // Create the pool in Edit mode.
            if(transform.childCount == 0)
                CreatePool(population, transform, toothPrefab);
        }
    }

  
}
