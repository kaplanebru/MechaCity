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
        
        public void RestorePreviousSelectionColors()
        {
            foreach (var actor in Actors)
            {
                foreach (var tower in ActorHolder.Registry[actor].Towers)
                {
                    AllTowers.GetData(tower).ColorHandler.ToSelectionColor();
                }
            }
        }

    }
}

