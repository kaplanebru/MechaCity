
using System;
using Enums;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    public BpData __data = new BpData();

    private void Start()
    {
         BpExample bpExample = new BpExample(BpType.Reverse,__data, new CombatAction()); 
    }

   
}

public class BpExample : BlueprintBase 
{
    public BpExample(BpType type, BpData data, IBpAction bpAction) : base(type, data, bpAction)
    {
        
    }
}
public class BpData
{
    
}

public abstract class BlueprintBase
{
    public BpType Type { get; private set; }
    public BpData Data { get; private set; }
    public IBpAction BpAction { get; private set; }
    
    
    public BlueprintBase(BpType type, BpData data, IBpAction bpAction)
    {
        Type = type;
        Data = data;
        BpAction = bpAction;
    }
    
    public void TryTakeAction()
    {
        BpAction?.Execute();
    }
}

public interface IBpAction
{
    void Execute();
}

public class CombatAction : IBpAction
{
    public void Execute()
    {
        Debug.Log("Reverse Order Action");
    }
}


//bp türleri: on combat, before combat