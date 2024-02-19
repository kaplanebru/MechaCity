
using Enums;
using UnityEngine;

public class Blueprint : MonoBehaviour
{
    
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
        BpAction?.TakeAction();
    }
}

public interface IBpAction
{
    void TakeAction();
}

public class CombatAction : IBpAction
{
    public void TakeAction()
    {
        Debug.Log("Reverse Order Action");
    }
}

//bp türleri: on combat, before combat