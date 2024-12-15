using System.Collections;
using System.Collections.Generic;
using Enums;
using Towers;
using UnityEngine;

namespace Towers
{
    public class DisarmData : BaseVisualSupportedData
    {
        public override VisualDataType Type { get; set; } = VisualDataType.Disarm;
        public override void SetVisually()
        {
            if(SatisfyRequirements())
                Eventbus.TowerEvents.OnDisarmamentActionTriggered?.Invoke(TowerID);
        }

        protected override bool SatisfyRequirements()
        {
            return Amount == 0;
        }
    }

}
