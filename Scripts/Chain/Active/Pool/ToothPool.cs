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

        Instance = this;
        ChainEvents.OnPoolCreated?.Invoke();
    }

    private void OnEnable()
    {
        if (!Application.isPlaying)
        {
            // Create the pool in Edit mode.
            CreatePool(population, transform, toothPrefab);
        }
    }

  
}
