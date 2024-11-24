using System.Collections;
using System.Collections.Generic;
using Actor;
using UnityEngine;

namespace Blueprint
{
    public class ShieldAction: IBpAction
    {
        public void Execute(params object[] obj)
        {
            Debug.Log("execute shield");
            var selectedActors = (uint[]) obj[0];
            var towers = ActorHolder.Registry[selectedActors[0]].TowerIDs;
            BpEventbus.ActionEvents.OnShieldActionTriggered?.Invoke(towers);
        }
    }
}
