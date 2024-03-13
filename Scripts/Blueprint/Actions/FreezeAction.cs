
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class FreezeAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            var selectedTowers = (int[]) obj[0];

            foreach (var selectedTower in selectedTowers)
            {
                AllTowers.GetTower(selectedTower).ToFreezeColor();
            }
        }

        public void Restore(params object[] obj)
        {
            var selectedTower = (int) obj[0];
            AllTowers.GetTower(selectedTower).ToSelectionColor();
            
        }
    }
}
