using System.Collections;
using System.Collections.Generic;
using Actor;
using Enums;
using Enums.Selections;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class BpShield : BaseBlueprint, IBpActionProcessor<ShieldAction>
    {
        public ShieldAction BpAction { get; } = new ShieldAction();
        public override BpType Type { get; set; } = BpType.Shield;
        public override SelectionType SelectionType { get; set; } = SelectionType.SinglePlayerOnlyBP;
        public override int Lifespan { get; set; } = 1; //COOLDOWN

        public override bool TryTakeAction(uint[] selectedItems)
        {
            if (CheckBpConstraints(selectedItems, out List<TowerData> towers))
            {
                IsPlaying = true;
                BpAction.Execute(towers);
                CompleteActionWithDelay();
                return true;
            }
            
            CompleteAction();
            return false;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
        }

        private bool CheckBpConstraints(uint[] selectedItems, out List<TowerData> availableTowers)
        {
            var actorID = selectedItems[0];
            var actor = ActorDB.Registry[actorID];
            
            availableTowers = new();
            
            foreach (var tower in actor.Towers)
            {
                ShieldData shieldData = tower.VisualData.VisualSupportedDatas[VisualDataType.Shield] as ShieldData;
                if(!shieldData.IsActive)
                    availableTowers.Add(tower);
            }
            
            return availableTowers.Count > 0;
        }
    }
}