using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "Selection/" + nameof(SelectionDataHolder), fileName = nameof(SelectionDataHolder))]
public class SelectionDataHolder : ScriptableObject
{
    public SelectionTypeDataCouple[] TypeDataCouples;
    public Dictionary<Selections.SelectionType, SelectionData> DataByType = new();

    public void Setup()
    {
        foreach (var couple in TypeDataCouples)
        {
            DataByType.Add(couple.Type, couple.Data);
        }
    }
}

[Serializable]
public class SelectionTypeDataCouple : TypeDataCouple<Selections.SelectionType, SelectionData>
{
}