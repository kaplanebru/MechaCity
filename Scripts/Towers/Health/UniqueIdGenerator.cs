using System;
using Towers;

public class UniqueIdGenerator
{
    private static int currentId = AllTowers.TowersCount + 10;
    public static int GenerateIntId()//todo: temporary
    {
        return currentId++;
    }
    public static string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString();
    }
}