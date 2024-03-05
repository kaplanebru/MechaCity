using System;
using System.Linq;
using System.Threading;
using Enums;
using Unity.Netcode;
using PlayerNetwork;
using Testing;


namespace Network
{
    public class TurnNetworkHandler : NetworkBehaviour
    {
        public NetworkVariable<TurnStateType> turnStateType = new(TurnStateType.Selection);
        public TeamType ownerTeamType;
        private Player _player;


        public override void OnNetworkSpawn()
        {
            turnStateType.OnValueChanged += CompleteStateBegin; //owner ve clone'u değişir
            

            if (IsOwner)
            {
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser += CompleteStateBeginServerRpc;
                NetworkEventbus.TriggerEvents.OnBpSelectionRequestByUser += ProcessBpSelectionServerRpc;
                
                NetworkEventbus.TurnEvents.OnTurnStarted += StartTurnServerRpc; //not: player 1'e mi bakıyor 2 pcde de

                NetworkEventbus.RequestEvents.OnPlayerSpawned += AssignPlayer; // //todo: fix later // playerın daha sonra assign olduğunu varsayıyor
            }
        }
        
        private void AssignPlayer(Player player, ulong arg2)
        {
            // _player = player;
            // ownerTeamType =  _player.Data.TeamType;
        }

        private void AssignPlayer2()
        {
            if (IsOwner)
            {
                _player = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>();
                ownerTeamType = _player.Data.TeamType;
            }
        }


        private void Start()
        {
            AssignPlayer2();
        }

        
        [ServerRpc]
        void StartTurnServerRpc(TeamType currentTeamType)
        {
            StartTurnClientRpc(currentTeamType);
        }

        [ClientRpc]
        void StartTurnClientRpc(TeamType currentTeamType)
        {
            // if (IsOwner)
            // {
            //     print("owner"); //TODO: test
            // }
            if(!IsOwner) return;
            if (currentTeamType != ownerTeamType) return;
            
            NetworkEventbus.RequestEvents.OnTurnButtonsShiftRequest?.Invoke();
           
            if(MultiplayerSetter.IsMultiplayerOn)
                _player.EnableInput(true);
        }
        
        [ServerRpc]
        private void ProcessBpSelectionServerRpc(BpType bpType)
        {
            ProcessBpSelectionClientRpc(bpType);
        }

        [ClientRpc]
        void ProcessBpSelectionClientRpc(BpType bpType)
        {
            print("owner");
            NetworkEventbus.RequestEvents.OnBpSelectionByServer?.Invoke(bpType);
            //2 ownera da 1 kez gidiyor
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
            
            // if (newvalue == TurnStateType.Exit)
            // {
            //     if (IsOwner)
            //         _player.EnableInput(false);
            // }
        }

        #endregion


        public override void OnNetworkDespawn()
        {
            turnStateType.OnValueChanged -= CompleteStateBegin;
            if (IsOwner)
            {
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser -= CompleteStateBeginServerRpc;
                NetworkEventbus.TriggerEvents.OnBpSelectionRequestByUser -= ProcessBpSelectionServerRpc;

                NetworkEventbus.TurnEvents.OnTurnStarted -= StartTurnServerRpc;
                NetworkEventbus.RequestEvents.OnPlayerSpawned -= AssignPlayer;
            }
        }
    }
}