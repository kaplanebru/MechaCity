using System.Collections;
using System.Collections.Generic;
using Actor;
using Blueprint;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class MultiShotAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            var selectedActors = (uint[]) obj[0];
            var towers = ActorHolder.Registry[selectedActors[0]].Towers;
            
            foreach (var tower in towers)
            {
                var attackData = tower.VisualSupportedDatas[VisualDataType.Attack];
                attackData.IncreaseDataAndVisuals(1);
            }
        }
    }

}
