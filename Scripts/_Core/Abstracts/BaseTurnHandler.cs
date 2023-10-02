using System.Collections.Generic;
using Enums;
using UnityEngine;
using Teams;

namespace Core
{
    public abstract class BaseTurnHandler : MonoBehaviour
    {
        public TurnAction turnAction;

        public Dictionary<string, Team> teams;
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
        public void SetTeams(Dictionary<string, Team> _teams)
        {
            teams = _teams;
        }
    
        public void ActionCompletedByUser()
        {
            Eventbus.NetworkTriggerEvents.OnCompleteActionRequestByUser?.Invoke(HandlerType);
        }
    
        public abstract void Unsubscribe();
        private void OnDisable()
        {
            TurnHelpers.ForEach(h=>h.enabled=false);
            Unsubscribe();
        }
    }
}
