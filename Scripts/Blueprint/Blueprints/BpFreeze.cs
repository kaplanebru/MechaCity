using System.Collections.Generic;
using Actor;
using Enums;
using Enums.Selections;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class BpFreeze : BaseBlueprint, IBpActionProcessor<FreezeAction> //bunlar scriptable olabilird, bpAction native olurdu? / bpnin monobehaviour olması gerekirdi
    {
        public override BpType Type { get; set; } = BpType.Freeze;
        public override SelectionType SelectionType { get; set; } = SelectionType.SingleRivalOnlyBP;
        public override int Lifespan { get; set; } = 1;  //dışardan belirlenmeli - değişken
        public FreezeAction BpAction { get; } = new FreezeAction();

        public override bool TryTakeAction(uint[] selectedItems)
        {
            if (CheckBpConstraints(selectedItems))
            {
                IsPlaying = true;
                Debug.Log("EXECUTE freeze");
                BpAction.Execute(selectedItems);
                CompleteActionWithDelay();
                return true;
            }
           
            CompleteAction();
            return false;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
            BpAction.Restore(selectedItem);
        }
        
        private bool CheckBpConstraints(uint[] selectedItems)
        {
            var actorID = selectedItems[0];
            var actor = ActorDB.Registry[actorID];

            return actor.ActivityStatus.CanMove;
        }
        
    }
}
