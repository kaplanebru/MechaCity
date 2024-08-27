using System;
using System.Collections.Generic;
using Enums;
using Towers;


namespace Turn
{
    [Serializable]
    public abstract class BaseTurnTransferData
    {
        public abstract TurnStateType StateType { get; set; }
        public abstract List<int> Towers { get; set; }
        
        public virtual void ResetPreviousTurnData()
        {
            Towers.Clear();
        }
        
        public void RestorePreviousSelectionColors()
        {
            Towers.ForEach(s=>AllTowers.GetData(s).ColorHandler.ToSelectionColor());
        }

    }
}

