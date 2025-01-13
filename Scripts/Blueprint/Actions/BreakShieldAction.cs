using System.Collections;
using System.Collections.Generic;
using Actor;
using Enums;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class BreakShieldAction : IBpAction
    {
        public BpType BPType { get; set; } = BpType.BreakShield;

        public void Execute(params object[] obj)
        {
            Debug.Log("execute break shield");
           
            var towers =  (List<TowerData>) obj[0];
            var selectedActors = (uint[]) obj[1];

            foreach (var tower in towers)
            {
                tower.VisualData.VisualSupportedDatas[VisualDataType.Shield].ResetDataOnly(0);
            }
            
            BpEventbus.ActionEvents.OnBreakShieldActionTriggered?.Invoke(ActorDB.Registry[selectedActors[0]].TowerIDs);
        }
    }

}
