using System;
using System.Linq;
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
                NetworkEventbus.TurnEvents.OnTurnStarted += StartTurnServerRpc; //not: player 1'e mi bakıyor 2 pcde de
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

        
        [ServerRpc]
        void StartTurnServerRpc(TeamType currentTeamType)
        {
            StartTurnClientRpc(currentTeamType);
        }

        [ClientRpc]
        void StartTurnClientRpc(TeamType currentTeamType)
        {
            if(!IsOwner) return;
            if (currentTeamType != ownerTeamType) return;
            
            //print(" start x " + player.NetworkObjectId); //bunu silince sapıtıyor, servera gitse ve serverdan seçilip yollansa belki düzelir
            
            NetworkEventbus.RequestEvents.OnTurnButtonsShiftRequest?.Invoke();
            player.EnableInput(true);
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
            
            if (newvalue == TurnStateType.Exit)
            {
                if (IsOwner)
                    player.EnableInput(false);
            }
        }

        #endregion


        public override void OnNetworkDespawn()
        {
            turnStateType.OnValueChanged -= CompleteStateBegin;
            if (IsOwner)
            {
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser -= CompleteStateBeginServerRpc;
                NetworkEventbus.TurnEvents.OnTurnStarted -= StartTurnServerRpc;
            }
        }
    }
}