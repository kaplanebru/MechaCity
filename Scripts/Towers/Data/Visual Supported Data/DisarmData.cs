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
            Eventbus.TowerEvents.OnDisarmamentActionTriggered?.Invoke(TowerID);
        }

        protected override bool SatisfyRequirements() => true;
    }

}
