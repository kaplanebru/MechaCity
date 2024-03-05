using System.Collections;
using System.Collections.Generic;
using Core;
using Teams;
using Towers;
using UnityEngine;

public class BpSelector : BaseSelector
{
    protected override void Select(int newSelection)
    {
        Towers.Add(newSelection);
        AllTowers.GetTower(newSelection).ToBlueprintColor();
    }
}
