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
                NetworkEventbus.TriggerEvents.OnCompleteStateRequestByUser += CompleteStateBeginServerRpc;
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
        void CompleteStateBeginServerRpc(TurnStateType lastType)
        {
            int nextType = ((int) lastType + 1) % Enum.GetValues(typeof(TurnStateType)).Length;
            turnStateType.Value = (TurnStateType) nextType;
        }
        
        private void CompleteStateBegin(TurnStateType previousvalue, TurnStateType newvalue)
        {
            //print("complete action : " + newvalue);

            if (newvalue != TurnStateType.Selection)
                NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer?.Invoke();
            else
                NetworkEventbus.RequestEvents.OnNewTurnRequest?.Invoke();
        }

        #endregion


        public override void OnNetworkDespawn()
        {
            turnStateType.OnValueChanged -= CompleteStateBegin;
            if (IsOwner)
            {
                NetworkEventbus.TriggerEvents.OnCompleteStateRequestByUser -= CompleteStateBeginServerRpc;
                NetworkEventbus.TurnEvents.OnTurnStarted -= TurnButtonsSetup;
            }
        }
    }
}