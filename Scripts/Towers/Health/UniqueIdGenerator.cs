using System;

public class UniqueIdGenerator
{
    public static string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString();
    }
}