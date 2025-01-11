using System.Collections;
using System.Collections.Generic;
using Actor;
using Blueprint;
using Enums;
using UnityEngine;

namespace Blueprint
{
    public class DisarmamentAction : IBpAction
    {
        public BpType BPType { get; set; } = BpType.Disarmament;

        public void Execute(params object[] obj)
        {
            var selectedActorID = (uint[]) obj[0];
            var selectedActor =ActorDB.Registry[selectedActorID[0]];
            var towers = selectedActor.Towers;

            selectedActor.ActivityStatus.CanShoot = false;
            
            foreach (var tower in towers)
            {
               tower.VisualData.VisualSupportedDatas[VisualDataType.Disarm].SetDataAndVisuals(0);
            }
        }
        
        public void Restore(params object[] obj)
        {
            var selectedActorID = (uint[]) obj[0];
            var selectedActor = ActorDB.Registry[selectedActorID[0]];
            var towers = selectedActor.Towers;
        
            selectedActor.ActivityStatus.CanShoot = true;
        }
    }

}
