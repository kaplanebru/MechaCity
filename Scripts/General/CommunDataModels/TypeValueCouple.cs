using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TypeValueCouple <TType, TValue> where TType : Enum
    where TValue : struct
{
    public TType Type;
    public TValue Value;
}
