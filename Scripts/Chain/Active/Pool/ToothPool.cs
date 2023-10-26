using System;
using Chain;
using GenericHelper;
using UnityEngine;

[ExecuteInEditMode]
public class ToothPool : Pool<Tooth>
{
    [SerializeField] int population = 1000;
    [SerializeField] Tooth toothPrefab;
    private void Awake() //instance awakete olunca editör bulamıyor
    {
    }

    private void OnEnable()
    {
        print("on pool enabled");
        Instance = this;
        CreatePool(population, transform, toothPrefab);
    }
    
}
