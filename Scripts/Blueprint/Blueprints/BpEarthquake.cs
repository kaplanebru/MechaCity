using System.Collections;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Blueprint
{
    public class BpEarthquake : BaseBlueprint, IBpActionProcessor<EarthquakeAction>
    {
        public EarthquakeAction BpAction { get; } = new EarthquakeAction();
        public override BpType Type { get; set; } = BpType.Earthquake;
        public override SelectionType SelectionType { get; set; } = SelectionType.None;
        public override int Lifespan { get; set; } = 1;
        public override bool TryTakeAction(uint[] selectedItems)
        {
            BpAction.Execute();
            return true;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
            
        }
    }

}
