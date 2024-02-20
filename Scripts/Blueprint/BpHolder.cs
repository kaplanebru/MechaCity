using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Network;
using UnityEngine;

public class BpHolder : MonoBehaviour
{
    public Dictionary<BpType, BaseBlueprint> Blueprints = new();
    //public Dictionary<BpType, IBpAction> BpActions = new(); //alternative
    
    private void OnEnable()
    {
        NetworkEventbus.BlueprintEvents.OnBpSelected += ExecuteBpAction;
    }

    private void ExecuteBpAction(BpType type)
    {
        Blueprints[type].BpAction.Execute();
        //BpActions[type].Execute();//alternative
    }

    void CreateBlueprints()
    {
        Blueprints.Add(BpType.Reverse, new BpReverse());
        Blueprints.Add(BpType.Freeze, new BpFreeze());
        //BpActions.Add(BpType.Reverse, new ReverseAction()); //alternative
    }

    private void OnDisable()
    {
        NetworkEventbus.BlueprintEvents.OnBpSelected -= ExecuteBpAction;
    }
}


public abstract class BaseBlueprint: IBpActionProcessor<IBpAction>
{
    public IBpAction BpAction { get; }
    public abstract BpType Type { get; set; }
}

public interface IBpActionProcessor<out TAction> where TAction : IBpAction
{
    public TAction BpAction { get; }
    public BpType Type { get; set; }
}


public interface IBpAction
{
    public void Execute();
}