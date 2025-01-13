using Enums;
using UnityEngine;

namespace Towers
{
    public class ShieldData : BaseVisualSupportedData
    {
        public override VisualDataType Type { get; set; } = VisualDataType.Shield;
        //actorde heightler farklı olabilir shieldler için
        public override bool ConvenientForInitialization()
        {
            return Amount > 0;
        }
        
        public override void SetVisually()
        {
            Eventbus.TowerEvents.OnShieldActionTriggered?.Invoke(TowerID, Amount);
        }
       
        public bool IsProtective(int towerHeight)
        {
            return towerHeight <= Amount;
        }
        
    }
}