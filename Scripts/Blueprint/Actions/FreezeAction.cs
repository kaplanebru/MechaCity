
using Actor;
using Enums;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class FreezeAction : IBpAction
    {
        public BpType BPType { get; set; } = BpType.Freeze;

        public void Execute(params object[] obj)
        {
            var selectedActorID = (uint[]) obj[0];
            var selectedActor = ActorDB.Registry[selectedActorID[0]];
            var selectedTowers = selectedActor.Towers;

            selectedActor.ActivityStatus.CanMove = false;
            
            foreach (var tower in selectedTowers)
            {
                tower.VisualData.ColorHandler.ToFreezeColor();
            }
            
        }

        public void Restore(params object[] obj)
        {
            Debug.Log("restore");
            var selectedActorID = (uint[]) obj[0];
            var selectedActor = ActorDB.Registry[selectedActorID[0]];
            var selectedTowers = selectedActor.Towers;
        
            selectedActor.ActivityStatus.CanMove = true;
            
            foreach (var tower in selectedTowers)
            {
                tower.VisualData.ColorHandler.SetDefaultTeamVisuals();
            }
        }
    }
}
