using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Towers
{
    [Serializable]
    public class TowerData
    {
        public TowerNumericData NumericData;
        public TowerVisualData VisualData;
        
        public TowerData(TowerNumericData numeric, TowerVisualData visual)
        {
            NumericData = numeric;
            VisualData = visual;
        }
        
        public void UpdateHeight(int extra)
        {
            if (extra == 0)
            {
                Debug.Log("EQUAL");
                return;
            }

            int newHeight = NumericData.Height + extra;
            SetHeightAutonomously(newHeight);
        }

        public void SetHeightAutonomously(int newHeight)
        {
            bool isRising = newHeight > NumericData.Height;
            NumericData.Height = newHeight;
            
            VisualData.Mover.SetHeightPhysically(newHeight, isRising);
        }
    }

}
