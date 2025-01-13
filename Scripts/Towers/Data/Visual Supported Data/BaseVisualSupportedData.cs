using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Towers
{
    public abstract class BaseVisualSupportedData
    {
        protected int TowerID;
        public abstract VisualDataType Type { get; set; }
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
            SetVisually();
        }

        public void IncreaseDataAndVisuals(int increaseAmount)
        {
            Amount += increaseAmount;
            SetVisually();
        }
        public abstract bool SatisfyRequirements();

        public void ResetDataOnly(int amount)
        {
            Amount = amount;
        }
    }
}
