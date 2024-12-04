using UnityEngine;

namespace Towers
{
    public class ShieldData : BaseVisualSupportedData
    {
        //actorde heightler farklı olabilir shieldler için
        protected override bool SatisfyRequirements()
        {
            return Amount > 0;
        }

        public override void SetVisually()
        {
            Eventbus.TowerEvents.OnShieldActionTriggered?.Invoke(TowerID, Amount);
        }
       
        public bool HasEffectiveShield(int towerHeight)
        {
            return towerHeight <= Amount;
        }

        public void ResetShieldDataOnly()
        {
            Amount = 0;
        }
    }
}