using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RiseFallData
{
    public int Id { get; private set; }
    public Transform ActiveHolder;
    public Transform PassiveHolder;
    public Transform Light;

    public List<Transform> PassiveParts = new();
    public List<Transform> ActiveParts = new();
    
    public RiseState RiseState;
    public float TargetHeight;

    public CommonData CommonData;

    public void SetId(int id)
    {
        Id = id;
    }
}