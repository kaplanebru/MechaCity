
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
                var tower = AllTowers.GetTower(selectedTower);
                
                tower.ToFreezeColor();
                tower.Data.BpTowerData.IsFreezing = true;
            }
        }

        public void Restore(params object[] obj)
        {
            var selectedTower = (int) obj[0];
            var tower = AllTowers.GetTower(selectedTower);
            
            tower.ToOriginalColor();
            tower.Data.BpTowerData.IsFreezing = false;
        }
    }
}
