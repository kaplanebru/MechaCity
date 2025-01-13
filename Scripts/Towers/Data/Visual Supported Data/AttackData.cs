using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Towers
{
    public class AttackData :BaseVisualSupportedData
    {
        public override VisualDataType Type { get; set; } = VisualDataType.Attack;
        public override void SetVisually()
        {
            if(!SatisfyRequirements()) return;
            Eventbus.TowerEvents.OnMultiShotActionTriggered?.Invoke(TowerID, Amount);
        }

        protected override bool SatisfyRequirements()
        {
            return Amount > 1;
        }

        public bool HasFilledMaxShotLimit()
        {
            return Amount == 3;
        }
    }

}
