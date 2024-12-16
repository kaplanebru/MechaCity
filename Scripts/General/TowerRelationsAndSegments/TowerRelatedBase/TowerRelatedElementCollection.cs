using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class TowerRelatedElementCollection<TRelatedElement> : MonoBehaviour where TRelatedElement : ITowerRelatedElement
{
    protected abstract Dictionary<int, TRelatedElement> Collection { get; set; }

    private void OnEnable()
    {
        GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet += GetItems;
        Subscribe();
    }

    public abstract void Subscribe();

    public abstract void Initialize();

    private void GetItems()
    {
        var items = GetComponentsInChildren<TRelatedElement>();
        foreach (var item in items)
        {
            Collection.Add(item.Id, item);
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