using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class TowerRelatedEventListener<TRelated> : MonoBehaviour where TRelated : ITowerRelated
{
    protected abstract TRelated[] RelatedItems { get; set; }
    private void OnEnable()
    {
        GeneralEventbus.InitializerEvents.OnTowersCreated += GetItems;
        Subscribe();
    }
    public abstract void Subscribe();

    public abstract void Initialize();

    private void GetItems()
    {
        RelatedItems = GetComponentsInChildren<TRelated>();
        Initialize();
    }

  

    public abstract void Unsubscribe();

    private void OnDisable()
    {
        GeneralEventbus.InitializerEvents.OnTowersCreated -= GetItems;
        Unsubscribe();

    }
}
