using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using Enums.Selections;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class BpDoubleSelf : BaseBlueprint, IBpActionProcessor<DoubleSelfAction>
    {
        public override BpType Type { get; set; } = BpType.DoubleSelf;
        public override SelectionType SelectionType { get; set; } = SelectionType.PlayerOnlyBp;
        public override int Lifespan { get; set; } = 1;
        public override int MaxSelectionAmount { get; set; } = 1;
        public DoubleSelfAction BpAction { get; } = new DoubleSelfAction();


        public override bool TryTakeAction(uint[] selectedItems)
        {
            if (CheckBpConstraints(selectedItems))
            {
                BpAction.Execute(selectedItems);
                return true;
            }

            Debug.Log("doesnt conform to constraints");
            //TODO: tekrar double'a yolla
            return false;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
            BpAction.Restore(selectedItem);
        }

        private bool CheckBpConstraints(uint[] selectedItems)
        {
            var actorID = selectedItems[0];
            var actor = ActorHolder.Registry[actorID];
            
            int counter = 0;
            foreach (var selectedItem in selectedItems)
            {
                if (selectedItem == actorID) continue;
                if (!actor.Neighbours.Contains(selectedItem))
                {
                    counter++;
                }
            }
            
            return counter != selectedItems.Length - 1;
        }

        ActorData[] ConvertToTowers(uint[] selectedItems)
        {
            ActorData[] actors = new ActorData[selectedItems.Length];
            for (var i = 0; i < selectedItems.Length; i++)
            {
                actors[i] = ActorHolder.GetActor(selectedItems[i]);
            }

            return actors;
        }
    }
}