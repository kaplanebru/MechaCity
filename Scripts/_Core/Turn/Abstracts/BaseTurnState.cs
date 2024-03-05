using System.Collections.Generic;
using System.Linq;
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

        public Dictionary<TeamState, Team> Teams;
        public abstract TurnStateType StateType { get; }
        public abstract int StateId { get; set; }
        
        public virtual void Register(){}
        
        public virtual void Unregister(){}
        public abstract void Subscribe();

        protected TurnManager turnManager;
        public void EnterState(TurnManager tturnManager = null)
        {
            turnManager = tturnManager;
            
            turnAction = TurnAction.Started;
            Subscribe();
        }

        public abstract void ProcessPreviousStateTransferData(BaseTurnTransferData data);

        public void CompleteState()
        {
            turnAction = TurnAction.Completed;
            Unsubscribe();
           // Debug.LogWarning("Unsubscribed from " + StateType);
        }
        public void SetTeams(Dictionary<TeamState, Team> teams)
        {
            Teams = teams;
        }

        public abstract void Unsubscribe();
        
    }
}
