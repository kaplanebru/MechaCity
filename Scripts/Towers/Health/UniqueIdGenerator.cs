using System;

public class UniqueIdGenerator
{
    private static int currentId = 0;
    public static int GenerateIntId()//todo: temporary
    {
        return currentId++;
    }
    public static string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString();
    }
}