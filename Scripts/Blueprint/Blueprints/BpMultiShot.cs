using System.Collections;
using System.Collections.Generic;
using Actor;
using Enums;
using Enums.Selections;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class BpMultiShot : BaseBlueprint, IBpActionProcessor<MultiShotAction>
    {
        public MultiShotAction BpAction { get; } = new MultiShotAction();
        public override BpType Type { get; set; } = BpType.MultiShot;
        public override SelectionType SelectionType { get; set; } = SelectionType.SinglePlayerOnlyBP;
        public override int Lifespan { get; set; } = 1;
        public override bool TryTakeAction(uint[] selectedItems)
        {
            if (CheckBpConstraints(selectedItems, out List<TowerData> towers))
            {
                IsPlaying = true;
                Debug.Log("execute multiShot");
                BpAction.Execute(towers); //buraya sadece selected towerı yolla
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
            
            if (!actor.ActivityStatus.CanShoot)
                return false;
            
            foreach (var tower in actor.Towers)
            {
                AttackData attackData = tower.VisualData.VisualSupportedDatas[VisualDataType.Attack] as AttackData;
                if(!attackData.HasFilledMaxShotLimit())
                    availableTowers.Add(tower);
            }
            
            return availableTowers.Count > 0;
        }
    }

}
