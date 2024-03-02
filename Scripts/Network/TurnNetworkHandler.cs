using System;
using Enums;
using Unity.Netcode;
using PlayerNetwork;


namespace Network
{
    public class TurnNetworkHandler : NetworkBehaviour
    {
        public NetworkVariable<TurnStateType> turnStateType = new(TurnStateType.Selection);
        public TeamType ownerTeamType;


        public override void OnNetworkSpawn()
        {
            turnStateType.OnValueChanged += CompleteStateBegin; //owner ve clone'u değişir

            if (IsOwner)
            {
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser += CompleteStateBeginServerRpc;
                NetworkEventbus.TurnEvents.OnTurnStarted += TurnButtonsSetup; //not: player 1'e mi bakıyor 2 pcde de
            }
        }


        private void Start()
        {
            if (IsOwner)
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
        void CompleteStateBeginServerRpc(TurnStateType nextType) //(TurnStateType lastType)
        {
            turnStateType.Value = nextType;
        }

        private void CompleteStateBegin(TurnStateType previousvalue, TurnStateType newvalue)
        {
            // if (newvalue != TurnStateType.Selection)
            NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer?.Invoke(newvalue);
            // else
            //     NetworkEventbus.RequestEvents.OnNewTurnRequest?.Invoke();
        }

        #endregion


        public override void OnNetworkDespawn()
        {
            turnStateType.OnValueChanged -= CompleteStateBegin;
            if (IsOwner)
            {
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser -= CompleteStateBeginServerRpc;
                NetworkEventbus.TurnEvents.OnTurnStarted -= TurnButtonsSetup;
            }
        }
    }
}