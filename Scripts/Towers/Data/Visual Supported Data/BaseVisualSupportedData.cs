using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Towers
{
    public abstract class BaseVisualSupportedData
    {
        protected int TowerID;
        public int Amount { get; internal set; } = 0;
        public abstract void SetVisually();
        
        public void Initialize(int towerID, int amount)
        {
            TowerID = towerID;
            Amount = amount;
        }
        
        public void SetDataAndVisuals(int amount)
        {
            Amount = amount;
            
            if(!SatisfyRequirements()) return;
            SetVisually();
        }

        protected abstract bool SatisfyRequirements();
    }
}
