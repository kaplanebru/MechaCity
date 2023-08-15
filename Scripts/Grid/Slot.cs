using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class Slot
{
    public bool hasTower;
    public int Number;
    public List<int> Pairs = new();
    public Tower Tower;
}
