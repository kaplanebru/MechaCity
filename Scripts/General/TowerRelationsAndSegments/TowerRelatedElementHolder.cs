using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class TowerRelatedElementHolder<TRelated> : MonoBehaviour where TRelated : ITowerRelatedElement
{
    protected abstract Dictionary<int, TRelated> RelatedItems { get; set; }

    private void OnEnable()
    {
        GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet += GetItems;
        Subscribe();
    }

    public abstract void Subscribe();

    public abstract void Initialize();

    private void GetItems()
    {
        var items = GetComponentsInChildren<TRelated>();
        foreach (var item in items)
        {
            RelatedItems.Add(item.Id, item);
        }
        Initialize();
    }


    public abstract void Unsubscribe();

    private void OnDisable()
    {
        GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet -= GetItems;
        Unsubscribe();
    }
}