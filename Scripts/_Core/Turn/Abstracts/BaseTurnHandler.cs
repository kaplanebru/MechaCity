using System.Collections.Generic;
using Enums;
using Grid;
using Network;
using UnityEngine;
using Teams;

namespace Turn
{
    public abstract class BaseTurnHandler : MonoBehaviour
    {
        public TurnAction turnAction;

        public Dictionary<string, Team> Teams;
        public Dictionary<TeamType, GameGrid> Grids;
        public abstract TurnHandlerType HandlerType { get; }
        public abstract void OnHandlerEnabled();
        public List<BaseTurnHelper> TurnHelpers = new();

        private void OnEnable()
        {
            TurnHelpers.ForEach(h=>h.enabled=true);
            turnAction = TurnAction.Started;
            OnHandlerEnabled();
        }
    
        public virtual void ProcessIncomingData(BaseTurnTransferData data){}
    
        public abstract void Setup();
    
        public void CompleteAction()
        {
            turnAction = TurnAction.Completed;
            enabled = false;
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
        private void OnDisable()
        {
            TurnHelpers.ForEach(h=>h.enabled=false);
            Unsubscribe();
        }
    }
}
