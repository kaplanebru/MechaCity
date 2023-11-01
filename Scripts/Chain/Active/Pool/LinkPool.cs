using GenericHelper;
using UnityEngine;


[ExecuteInEditMode]

public class LinkPool : Pool<Transform>
{
    [SerializeField] int population = 100;
    [SerializeField] Transform linkPrefab;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreatePool(population, transform, linkPrefab);
    }

 
}
