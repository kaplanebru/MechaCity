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
            IsActive = true;
        }

        public override bool ConvenientForInitialization() //bu boşa çıkıyor
        {
            return false;
        }

        public bool IsDisarmed()
        {
            return Amount > 0;
        }
        
        
    }

}
