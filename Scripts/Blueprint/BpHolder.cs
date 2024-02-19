using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Network;
using UnityEngine;

public class BpHolder : MonoBehaviour
{
    public Dictionary<BpType, BaseBlueprint> BpActions = new(); //TODO: Silme, drive'a ekle BaseBlueprint üzerinde generic olursa inherit olan classları listeye almıyor
    //  public Dictionary<BpType, BaseBlueprint<IBpAction>> BpActions = new Dictionary<BpType, BaseBlueprint<IBpAction>>();
    
    private void OnEnable()
    {
        NetworkEventbus.BlueprintEvents.OnBpSelected += ExecuteBpAction;
    }

    private void ExecuteBpAction(BpType type)
    {
        BpActions[type].TryTakeAction();
    }

    void CreateBlueprints()
    {
        BpActions.Add(BpType.Reverse, new BpReverse());
        BpActions.Add(BpType.Freeze, new BpFreeze());
    }
}

public class BpReverse : BaseBlueprint, IBpActionHolder<ReverseAction>
{
    public ReverseAction BpAction { get; } = new();

    public override BpType Type { get; set; } = BpType.Reverse;
    public override void TryTakeAction()
    {
        BpAction?.Execute();
    }
}

public class BpFreeze : BaseBlueprint, IBpActionHolder<FreezeAction>
{
    public FreezeAction BpAction { get; }
    public override BpType Type { get; set; }
    public override void TryTakeAction()
    {
        BpAction?.Execute();
    }
}

public interface IBpActionHolder<out TAction> where TAction : IBpAction
{
    public TAction BpAction { get; }
    public BpType Type { get; set; }

}