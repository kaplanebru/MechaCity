using Chain;
using GenericHelper;
using UnityEngine;

public class ToothPool : Pool<Tooth>
{
    [SerializeField] int population = 1000;
    [SerializeField] Tooth toothPrefab;
    private void Awake()
    {
        Instance = this;
        
    }

    private void Start()
    {
        CreatePool(population, transform, toothPrefab);
    }
}
