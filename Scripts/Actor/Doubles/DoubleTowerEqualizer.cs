using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Towers
{
    public static class DoubleTowerEqualizer
    {
        public static void Equalize(TowerData[] towers) //bridgeden önce olmalı
        { 
            TowerNumericData[] datas = towers.Select(t => t.NumericData).ToArray();
            int amount = towers.Length;
            int totalHeight = 0;
            foreach (var tower in datas)
            {
                totalHeight += tower.Height;
            }

            int averageHeight = totalHeight / amount;
            int rest = totalHeight % averageHeight;

            for (var i = datas.Length - 1; i >= 0; i--)
            {
                var data = datas[i];
                int extra = 0;
                if (rest > 0)
                {
                    extra = 1;
                    rest--;
                }

                var newHeight = averageHeight + extra;
                if (newHeight == data.Height) continue;

                int surplus = newHeight - data.Height;

                if (surplus == 0) continue;
                towers[data.UniqID].UpdateHeight(surplus);
                AllTowers.GetTower(data.UniqID).StartRiseFallRoutine(true); //Todo: düzelt
            }
        }
   
    }

}
