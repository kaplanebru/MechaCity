using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;

namespace Blueprint
{
    public class BreakShieldAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            Debug.Log("execute break shield");
            var selectedActors = (uint[]) obj[0];
            var towers =ActorHolder.Registry[selectedActors[0]].Towers;

            foreach (var tower in towers)
            {
                tower.ShieldData.ResetShield();
            }
            BpEventbus.ActionEvents.OnBreakShieldActionTriggered?.Invoke(ActorHolder.Registry[selectedActors[0]].TowerIDs);
        }
    }

}
