using System.Collections.Generic;
using Enums;
using Teams;

namespace Turn
{
    public abstract class BaseTurnState
    {
        public TurnAction turnAction;

        public Dictionary<TeamState, Team> TeamsByTurn; //todo: güncelleniyor mu?
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
            Eventbus.TurnStateEvents.OnTurnStateBegin?.Invoke(StateType);
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
            TeamsByTurn = teams;
        }
        
        // public virtual void TryExecuteBp(){}

        public abstract void Unsubscribe();
        
        public abstract void UnsubscribeFromConstantEvents();
    }
}