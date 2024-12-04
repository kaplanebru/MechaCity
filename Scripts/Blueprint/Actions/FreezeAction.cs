
using Actor;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class FreezeAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            var selectedActors = (uint[]) obj[0];
            var selectedTowers = ActorHolder.Registry[selectedActors[0]].Towers;
            
            foreach (var tower in selectedTowers)
            {
                tower.ColorHandler.ToFreezeColor();
                tower.BpTowerData.IsFreezing = true;
            }
        }

        public void Restore(params object[] obj)
        {
            var selectedTower = (int) obj[0];
            var tower = AllTowers.GetData(selectedTower);
            
            tower.ColorHandler.ToOriginalColor();
            tower.BpTowerData.IsFreezing = false;
        }
    }
}
