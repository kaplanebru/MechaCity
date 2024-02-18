using System;
using Enums;
using Unity.Netcode;
using PlayerNetwork;


namespace Network
{
    public class TurnNetworkHandler : NetworkBehaviour
    {
        public NetworkVariable<TurnStateType> turnHandlerType = new(TurnStateType.Selection);
        public TeamType ownerTeamType;


        public override void OnNetworkSpawn()
        {
            turnHandlerType.OnValueChanged += CompleteActionSetup; //owner ve clone'u değişir
            
            if (IsOwner)
            {
                NetworkEventbus.TurnEvents.OnTurnEnding += RequestNewTurnServerRpc;
                NetworkEventbus.TriggerEvents.OnCompleteActionRequestByUser += CompleteActionSetupServerRpc;
                NetworkEventbus.TurnEvents.OnTurnStarted += TurnButtonsSetup;  //not: player 1'e mi bakıyor 2 pcde de
            }
        }
        

        private void Start()
        {
            if(IsOwner)
                ownerTeamType = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>().Data.TeamType;
        }

        void TurnButtonsSetup(TeamType currentTeamType)
        {
            //print("owner team type: " + ownerTeamType + " currentTeamType: " + currentTeamType);
            if (currentTeamType == ownerTeamType)
            {
                NetworkEventbus.RequestEvents.OnTurnButtonsShiftRequest?.Invoke();
            }
        }


        #region Complete Turn Handle

        [ServerRpc]
        void CompleteActionSetupServerRpc(TurnStateType lastType)
        {
            int nextType = ((int) lastType + 1) % Enum.GetValues(typeof(TurnStateType)).Length;
            turnHandlerType.Value = (TurnStateType) nextType;
        }

        [ServerRpc]
        void RequestNewTurnServerRpc()
        {
            turnHandlerType.Value = TurnStateType.Selection;
        }

        private void CompleteActionSetup(TurnStateType previousvalue, TurnStateType newvalue)
        {
            //print("complete action : " + newvalue);

            if (newvalue != TurnStateType.Selection)
                NetworkEventbus.RequestEvents.OnCompleteActionRequest?.Invoke();
            else
                NetworkEventbus.RequestEvents.OnNewTurnRequest?.Invoke();
        }

        #endregion


        public override void OnNetworkDespawn()
        {
            turnHandlerType.OnValueChanged -= CompleteActionSetup;
            if (IsOwner)
            {
                NetworkEventbus.TurnEvents.OnTurnEnding -= RequestNewTurnServerRpc;
                NetworkEventbus.TriggerEvents.OnCompleteActionRequestByUser -= CompleteActionSetupServerRpc;
                NetworkEventbus.TurnEvents.OnTurnStarted -= TurnButtonsSetup;
            }
        }
    }
}