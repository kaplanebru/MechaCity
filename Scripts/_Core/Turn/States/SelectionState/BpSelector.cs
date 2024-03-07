using System.Collections;
using System.Collections.Generic;
using Core;
using Enums;
using Teams;
using Towers;
using Turn;
using UnityEngine;

public class BpSelector : BaseSelector
{
    public sealed override TurnStateType StateType { get; set; }
    public BpSelector(TurnStateType stateType) : base(stateType)
    {
        StateType = stateType;
    }

    protected override void Select(int newSelection)
    {
        Towers.Add(newSelection);
        AllTowers.GetTower(newSelection).ToBlueprintColor();
    }

   
}
