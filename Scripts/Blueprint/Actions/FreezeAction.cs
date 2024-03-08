
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class FreezeAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            int[] selectedTowers = (int[]) obj[0];

            foreach (var selectedTower in selectedTowers)
            {
                AllTowers.GetTower(selectedTower).ToFreezeColor();
            }
        }
    }
}
