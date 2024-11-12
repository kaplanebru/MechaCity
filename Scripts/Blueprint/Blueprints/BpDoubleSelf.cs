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

        private bool CheckBpConstraints(uint[] selectedItems) //yanında olup olmadığına bakıyor double'ın, dışardan da check edilebilir
        {
            // actors = actors.OrderBy(a => a.ID).ToArray();

            for (var i = 0; i < selectedItems.Length; i++)
            {
                var actorID = selectedItems[i];
                var nextActor = selectedItems[(i + 1) % (selectedItems.Length)];

                int previousIndex = selectedItems.Length - 1;
                if (i - 1 >= 0)
                {
                    previousIndex = i - 1;
                }

                var previousActor = selectedItems[previousIndex];

                var actor = ActorHolder.Registry[actorID];

                if (!actor.Neighbours.Contains(nextActor) && !actor.Neighbours.Contains(previousActor))
                    return false;
            }

            return true;
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