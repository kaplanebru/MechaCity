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
        private Player player;


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
            {
                player = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>();
                ownerTeamType = player.Data.TeamType;
                
            }

        }

        void TurnButtonsSetup(TeamType currentTeamType)
        {
         

            player.EnableInput(false);
            print(" stop x " + player.NetworkObjectId); 
            
            if (currentTeamType == ownerTeamType)
            {
                print(" start x " + player.NetworkObjectId); //sadece ownerlarda çalışıyor: 2 tarafta da değil gibi

                NetworkEventbus.RequestEvents.OnTurnButtonsShiftRequest?.Invoke();
                player.EnableInput(true);
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
            NetworkEventbus.RequestEvents.OnCompleteStateRequestByServer?.Invoke(newvalue);
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