using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "Selection/" + nameof(SelectionAssetHolder), fileName = nameof(SelectionAssetHolder))]
public class SelectionAssetHolder : ScriptableObject
{
    public SelectionTypeDataCouple[] TypeDataCouples;
    public Dictionary<SelectionType, SelectionData> DataByType = new();

    public void Setup()
    {
        foreach (var couple in TypeDataCouples)
        {
            DataByType.Add(couple.Type, couple.Data);
        }
    }
}

[Serializable]
public class SelectionTypeDataCouple : TypeDataCouple<SelectionType, SelectionData>
{
}