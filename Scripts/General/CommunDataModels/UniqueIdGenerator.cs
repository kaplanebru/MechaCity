using System;
public class UniqueIdGenerator
{
    private static int currentId = 100;//AllTowers.TowersCount + 10;
    public static int GenerateIntId()//todo: temporary
    {
        return currentId++;
    }
    public static string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString();
    }
}