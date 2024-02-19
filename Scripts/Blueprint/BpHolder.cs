using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Network;
using UnityEngine;

public class BpHolder : MonoBehaviour
{
    
    public Dictionary<BpType, BaseBlueprint<IBpAction>> BpActions = new Dictionary<BpType, BaseBlueprint<IBpAction>>();
    
    
  //  public Dictionary<BpType, BaseBlueprint<IBpAction>> BpActions = new Dictionary<BpType, BaseBlueprint<IBpAction>>();


 
    
    private List<BaseBlueprint<IBpAction>> Test = new List<BaseBlueprint<IBpAction>>();



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
        //BpActions.Add(BpType.Reverse, new BpReverse());
      

    }
}


public class BpReverse : BaseBlueprint<ReverseAction>
{
    public override ReverseAction BpAction { get; set; } = new ();
    
    public override BpType Type { get; set; } = BpType.Reverse;
}

