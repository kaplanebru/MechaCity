using System.Collections.Generic;
using Enums;
using Grid;
using Network;
using UnityEngine;
using Teams;

namespace Turn
{
    public abstract class BaseTurnState 
    {
        public TurnAction turnAction;

        public Dictionary<string, Team> Teams;
        public abstract TurnStateType StateType { get; }
        public abstract int StateId { get; set; }
        
        public abstract void Subscribe();

        protected TurnManager turnManager;
        public void EnterState(TurnManager turnManager = null)
        {
            this.turnManager = turnManager;
            
            turnAction = TurnAction.Started;
            Subscribe();
            StartState();
        }
        
        public virtual void ProcessPreviousStateTransferData(BaseTurnTransferData data){}
    
        public abstract void StartState();
    
        public void CompleteState()
        {
            turnAction = TurnAction.Completed;
            Unsubscribe();
            //Debug.LogWarning("Unsubscribed from " + StateType);
        }
        public void SetTeams(Dictionary<string, Team> teams)
        {
            Teams = teams;
        }

        public abstract void ResetPreviousTurnData();

        public abstract void RestorePreviousSelectionColors();
    
        public abstract void Unsubscribe();
        
    }
}
