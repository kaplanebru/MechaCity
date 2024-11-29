using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class ShieldAction: IBpAction
    {
        public void Execute(params object[] obj)
        {
            Debug.Log("execute shield");
            var selectedActors = (uint[]) obj[0];
            var towers = ActorHolder.Registry[selectedActors[0]].Towers;

            foreach (var tower in towers)
            {
                tower.ShieldData.SetShield(tower.Height);
            }
        }
    }
}
