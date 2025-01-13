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
            if (CheckBpConstraints(selectedItems))
            {
                IsActive = true;

                BpAction.Execute(selectedItems);
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

        private bool CheckBpConstraints(uint[] selectedItems)
        {
            var actorID = selectedItems[0];
            var actor = ActorDB.Registry[actorID];
            var tower = actor.Towers[0];

            ShieldData shieldData = tower.VisualData.VisualSupportedDatas[VisualDataType.Shield] as ShieldData;
            return !shieldData.HasEffectiveShield(tower.NumericData.Height);
        }
    }
}