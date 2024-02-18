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
        public Dictionary<TeamType, GameGrid> Grids;
        public abstract TurnHandlerType HandlerType { get; }
        public abstract int StateId { get; set; }
        public abstract void OnHandlerEnabled();
        public List<BaseTurnHelper> TurnHelpers = new();

        
        // public void Subscribe()
        // {
        //     //TurnHelpers.ForEach(h=>h.enabled=true);
        //     turnAction = TurnAction.Started;
        // }

        public abstract void Subscribe();

        public void EnterState(TurnManager turnManager)
        {
            turnAction = TurnAction.Started;
            Subscribe();
            Setup();
        }
        
        public abstract void UpdateState(TurnManager turnManager);
        
    
        public virtual void ProcessIncomingData(BaseTurnTransferData data){}
    
        public abstract void Setup();
    
        public void CompleteAction()
        {
            turnAction = TurnAction.Completed;
            Unsubscribe();
            //enabled = false;
        }
        public void SetTeams(Dictionary<string, Team> teams)
        {
            Teams = teams;
        }
    
        public void ActionCompletedByUser()
        {
            NetworkEventbus.TriggerEvents.OnCompleteActionRequestByUser?.Invoke(HandlerType);
        }
    
        public abstract void Unsubscribe();
        // private void OnDisable()
        // {
        //     TurnHelpers.ForEach(h=>h.enabled=false);
        //     Unsubscribe();
        // }
    }
}
