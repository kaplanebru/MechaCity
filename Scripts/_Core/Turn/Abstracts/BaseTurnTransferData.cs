using System;
using System.Collections.Generic;
using Actor;
using Enums;
using Towers;


namespace Turn
{
    [Serializable]
    public abstract class BaseTurnTransferData
    {
        public abstract TurnStateType StateType { get; set; }
        public abstract List<uint> Actors { get; set; }
        
        public virtual void ResetPreviousTurnData()
        {
            Actors.Clear();
        }
        
        public void RestorePreviousSelectionColors() //TODO: burda actor değil tower tutulmalı!!
        {
            foreach (var actorID in Actors)
            {
                foreach (var tower in ActorDB.GetTowerIDs(actorID))
                {
                    AllTowers.GetData(tower).VisualData.ColorHandler.ToSelectionColor();
                }
            }
        }

    }
}

