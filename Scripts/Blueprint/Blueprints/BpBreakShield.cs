using System.Collections;
using System.Collections.Generic;
using Actor;
using Enums;
using Enums.Selections;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class BpBreakShield :BaseBlueprint, IBpActionProcessor<BreakShieldAction>
    {
        public BreakShieldAction BpAction { get; } = new BreakShieldAction();
        public override BpType Type { get; set; } = BpType.BreakShield;
        public override SelectionType SelectionType { get; set; } = SelectionType.SingleRivalOnlyBP;
        public override int Lifespan { get; set; } = 1;
        public override bool TryTakeAction(uint[] selectedItems)
        {
            if (CheckBpConstraints(selectedItems, out List<TowerData> availableTowers))
            {
                IsActive = true;
                BpAction.Execute(availableTowers, selectedItems);
                DeselectItems();
                return true;
            }
            
            DeselectItems();
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
                if (shieldData.IsActive)
                    availableTowers.Add(tower);
            }

            return availableTowers.Count > 0;
        }
    }
}

