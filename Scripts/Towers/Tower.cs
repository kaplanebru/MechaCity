using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Towers
{
    public class Tower
    {
        public TowerNumericData NumericData;
        public TowerData VisualData;
        
        public Tower(TowerNumericData numeric, TowerData visual)
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
            bool isRising = newHeight > NumericData.Height;
            NumericData.Height = newHeight;

            VisualData.Mover.ChangeHeightPhysically(newHeight, isRising);
        }
    }

}
