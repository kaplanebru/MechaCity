using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


        public override bool TryTakeAction(int[] selectedItems)
        {
            if (CheckSelectionConstraints(selectedItems))
            {
                BpAction.Execute(selectedItems);
                return true;
            }
            
            Debug.Log("doesnt conform to constraints");
            return false;
        }

        public override void TryRestoreAction(int selectedItem)
        {
            BpAction.Restore(selectedItem);
        }

        TowerData[] ConvertToTowers(int[] selectedItems)
        {
            TowerData[] towers = new TowerData[selectedItems.Length];
            for (var i = 0; i < selectedItems.Length; i++)
            {
                towers[i] = AllTowers.GetData(selectedItems[i]);
            }

            return towers;
        }

        public bool CheckSelectionConstraints(int[] selectedItems)
        {
            var towers = ConvertToTowers(selectedItems);
            towers = towers.OrderBy(t => t.UniqID).ToArray();

            for (var i = 0; i < towers.Length; i++)
            {
                var tower = towers[i];
                var nextTower = towers[(i + 1) % (towers.Length)];
                if (tower.NeighbourIDs[1] != nextTower.UniqID)
                    return false;
            }

            return true;
        }
    }
}