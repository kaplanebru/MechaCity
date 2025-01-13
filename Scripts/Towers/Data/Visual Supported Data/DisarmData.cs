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

        public override bool SatisfyRequirements() //bu boşa çıkıyor
        {
            return Amount == 0;
        }

        public bool HasDisarmed()
        {
            return Amount > 0;
        }
    }

}
