
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class FreezeAction : IBpAction
    {
        private int[] selectedTowers;
        public void Execute(params object[] obj)
        {
            selectedTowers = (int[]) obj[0];

            foreach (var selectedTower in selectedTowers)
            {
                AllTowers.GetTower(selectedTower).ToFreezeColor();
            }
        }

        public void Restore()
        {
            foreach (var selectedTower in selectedTowers)
            {
                AllTowers.GetTower(selectedTower).ToSelectionColor();
            }
        }
    }
}
