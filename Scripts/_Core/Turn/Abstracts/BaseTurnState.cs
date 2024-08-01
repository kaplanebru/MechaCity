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

        public virtual void Register()
        {
        }

        public abstract void SubscribeToConstantEvents();
        public abstract void Subscribe();

        public void EnterState(TurnManager tturnManager = null)
        {
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
        
        public virtual void ExecuteSelection(){}

        public abstract void Unsubscribe();
        
        public abstract void UnsubscribeFromConstantEvents();
    }
}