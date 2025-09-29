using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;


[CreateAssetMenu(menuName = "Blueprint/" + nameof(BPTimingData), fileName = nameof(BPTimingData))]
public class BPTimingData : ScriptableObject
{
    [SerializeField] private TypeValueCouple<BpType, float>[] serializedDurationByType;
    public Dictionary<BpType, float> DurationByType = new();

    private void OnEnable()
    {
        Setup();
    }

    void Setup()
    {
        foreach (var pair in serializedDurationByType)
        {
            DurationByType.Add(pair.Type, pair.Value);
        }
    }
}