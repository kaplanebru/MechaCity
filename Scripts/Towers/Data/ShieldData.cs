using UnityEngine;

namespace Towers
{
    public class ShieldData
    {
        //actorde heightler farklı olabilir shieldler için
        public int Height { get; private set; } = 0;
        private int TowerID;


        public  void Initialize(int towerID, int height)
        {
            TowerID = towerID;
            Height = height;
        }
        public void SetPhysically()
        {
            Eventbus.TowerEvents.OnShieldActionTriggered?.Invoke(TowerID, Height);
        }
        public void SetShield(int height)
        {
            Height = height;
            
            if(height <= 0) return;
            SetPhysically();
        }
        
        public bool HasEffectiveShield(int towerHeight)
        {
            return towerHeight <= Height;
        }

        public void ResetShield()
        {
            Height = 0;
        }
    }
}