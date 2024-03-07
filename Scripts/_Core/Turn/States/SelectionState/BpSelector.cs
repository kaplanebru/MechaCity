using System.Collections;
using System.Collections.Generic;
using Core;
using Teams;
using Towers;
using Turn;
using UnityEngine;

public class BpSelector : BaseSelector<IntruderTransferData>
{
    public override IntruderTransferData turnData { get; set; } = new IntruderTransferData();

    protected override void Select(int newSelection)
    {
        Towers.Add(newSelection);
        AllTowers.GetTower(newSelection).ToBlueprintColor();
    }
}
