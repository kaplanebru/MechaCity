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
            if (CheckSelectionConstraints(selectedItems))
            {
                BpAction.Execute(selectedItems);
                return true;
            }
            
            Debug.Log("doesnt conform to constraints");
            return false;
        }

        public override void TryRestoreAction(uint selectedItem)
        {
            BpAction.Restore(selectedItem);
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

        public bool CheckSelectionConstraints(uint[] actors)
        {
           
           // actors = actors.OrderBy(a => a.ID).ToArray();

            for (var i = 0; i < actors.Length; i++)
            {
                var actor = actors[i];
                var nextActor = actors[(i + 1) % (actors.Length)];

                // if (!actor.NeighbourIDs.Contains(nextActor.ID)) //todo: neighbours
                //     return false;
            }

            return true;
        }
    }
}