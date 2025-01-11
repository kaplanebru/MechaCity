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
        public BpType BPType { get; set; } = BpType.MultiShot;

        public void Execute(params object[] obj)
        {
            var selectedActors = (uint[]) obj[0];
            var towers = ActorDB.Registry[selectedActors[0]].Towers;
            
            foreach (var tower in towers)
            {
                var attackData = tower.VisualData.VisualSupportedDatas[VisualDataType.Attack];
                attackData.IncreaseDataAndVisuals(1);
            }
        }
    }

}
