using System;
using UnityEngine;

public class UniqueIdGenerator
{
    private static uint uintId = 0;
    private static int intId = 0;
    
  
    public static string StringId()
    {
        return Guid.NewGuid().ToString();
    }
    public static uint UIntId()
    {
        Debug.Log(uintId+1);
        return uintId++;
    }
    public static int IntId()
    {
        return intId++;
    }
}