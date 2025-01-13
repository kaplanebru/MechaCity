using System.Collections;
using System.Collections.Generic;
using Actor;
using Blueprint;
using Enums;
using Enums.Selections;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class BpDisarmament :BaseBlueprint, IBpActionProcessor<DisarmamentAction>
    {
        public DisarmamentAction BpAction { get; } = new DisarmamentAction();
        public override BpType Type { get; set; } = BpType.Disarmament;
        public override SelectionType SelectionType { get; set; } = SelectionType.SingleRivalOnlyBP;
        public override int Lifespan { get; set; } = 1;
        public override bool TryTakeAction(uint[] selectedItems)
        {
            if (CheckBpConstraints(selectedItems, out List<TowerData> availableTowers))
            {
                IsPlaying = true;
                BpAction.Execute(availableTowers, selectedItems);
                DeselectItems();
                return true;
            }
           
            DeselectItems();
            CompleteAction();
            return false;
        }

        public override void TryRestoreAction(uint selectedItem) {}
        
        private bool CheckBpConstraints(uint[] selectedItems, out List<TowerData> availableTowers)
        {
            var actorID = selectedItems[0];
            var actor = ActorDB.Registry[actorID];
            
            availableTowers = new();
            
            foreach (var tower in actor.Towers)
            {
                DisarmData disarmData = tower.VisualData.VisualSupportedDatas[VisualDataType.Disarm] as DisarmData;
                if(!disarmData.IsActive)
                    availableTowers.Add(tower);
            }
            
            return availableTowers.Count > 0;
        }
    }

}

