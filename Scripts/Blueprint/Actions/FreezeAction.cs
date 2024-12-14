
using Actor;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class FreezeAction : IBpAction
    {
        public void Execute(params object[] obj)
        {
            var selectedActorID = (uint[]) obj[0];
            var selectedActor = ActorHolder.Registry[selectedActorID[0]];
            var selectedTowers = selectedActor.Towers;

            selectedActor.ActivityStatus.CanMove = false;
            
            foreach (var tower in selectedTowers)
            {
                tower.ColorHandler.ToFreezeColor();
            }
        }

        public void Restore(params object[] obj)
        {
            var selectedActorID = (uint[]) obj[0];
            var selectedActor = ActorHolder.Registry[selectedActorID[0]];
            var selectedTowers = selectedActor.Towers;
        
            selectedActor.ActivityStatus.CanMove = true;
            
            foreach (var tower in selectedTowers)
            {
                tower.ColorHandler.ToOriginalColor();
            }
        }
    }
}
