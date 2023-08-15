using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireData : BaseTurnData
{
    
}

public class FireHandler : BaseTurnHandler, ITurnActionHandler<FireData>
{
    public FireData Data { get; private set; }

    public override void Subscribe()
    {
        Data = new();
    }
    
    



    public override void Unsubscribe()
    {
    }
}
