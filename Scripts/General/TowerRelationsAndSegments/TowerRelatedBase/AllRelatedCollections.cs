using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TowerRelatedType
{
    Floor,
    Shield,
    DisarmSign,
    MultiShooter,
    Shooter,
    Health,
    Lock,
    Bridge,
}
public class AllRelatedCollections : MonoBehaviour
{
    private Dictionary<TowerRelatedType, TowerRelatedCollection> CollectionRegistry { get; set; } = new();

    void RegisterCollections()
    {
        CollectionRegistry.Add(TowerRelatedType.Floor, new TowerRelatedCollection());
        //CollectionRegistry[TowerRelatedType.Floor].RegisterCollection(GetComponentsInChildren<Floor>());
        //CollectionRegistry[TowerRelatedType.Shooter].RegisterCollection(GetComponentsInChildren<>());
    }
}

public class TowerRelatedCollection
{
    private ITowerRelatedElement[] Crowd;
    public Dictionary<int, ITowerRelatedElement> Collection = new();

    public void RegisterCollection(ITowerRelatedElement[] newCrowd)
    {
        Crowd = newCrowd;
        foreach (var item in Crowd)
        {
            Collection.Add(item.Id, item);
        }
    }
}

