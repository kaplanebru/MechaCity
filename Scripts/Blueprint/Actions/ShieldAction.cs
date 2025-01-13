using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class ShieldAction: IBpAction
    {
        public BpType BPType { get; set; } = BpType.Shield;

        public void Execute(params object[] obj)
        {
            Debug.Log("execute shield");
            var towers = (List<TowerData>) obj[0];

            for (var i = 0; i < towers.Count; i++)
            {
                var tower = towers[i];
                var towerNumeric = tower.NumericData;
                tower.VisualData.VisualSupportedDatas[VisualDataType.Shield].SetDataAndVisuals(towerNumeric.Height);
            }
            
            BpEventbus.ActionEvents.OnBpActionCompleteRequest?.Invoke(BPType);
        }
    }
}
