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
            var selectedActors = (uint[]) obj[0];
            var towers = ActorDB.Registry[selectedActors[0]].Towers;
            var towerNumericDatas = ActorDB.Registry[selectedActors[0]].TowerNumericDatas;

            for (var i = 0; i < towers.Length; i++)
            {
                var tower = towers[i];
                var towerNumeric = towerNumericDatas[i];
                tower.VisualData.VisualSupportedDatas[VisualDataType.Shield].SetDataAndVisuals(towerNumeric.Height);
            }
            
            BpEventbus.ActionEvents.OnBpActionCompleteRequest?.Invoke(BPType);
        }
    }
}
