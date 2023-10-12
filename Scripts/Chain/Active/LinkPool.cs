using System;
using System.Collections;
using System.Collections.Generic;
using GenericHelper;
using UnityEngine;

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

    // public Transform GetLinkPrefab()
    // {
    //     return linkPrefab;
    // }
}
