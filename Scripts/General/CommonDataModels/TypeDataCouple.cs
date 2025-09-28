using System;

[Serializable]
public class TypeDataCouple<TType, TData>
    where TType : Enum
    where TData : class
{
    public TType Type;
    public TData Data;
}