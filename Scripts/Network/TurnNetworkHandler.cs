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
        public NetworkVariable<TurnStateType> turnStateType = new(TurnStateType.Exit);
        //_player = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>();


        public override void OnNetworkSpawn()
        {
            turnStateType.OnValueChanged += CompleteStateBegin; //owner ve clone'u değişir
            

            if (IsOwner)
            {
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser += CompleteStateBeginServerRpc;
                NetworkEventbus.TriggerEvents.OnBpSelectionRequestByUser += ProcessBpSelectionServerRpc;

            }
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
        }

        #endregion


        public override void OnNetworkDespawn()
        {
            turnStateType.OnValueChanged -= CompleteStateBegin;
            if (IsOwner)
            {
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser -= CompleteStateBeginServerRpc;
                NetworkEventbus.TriggerEvents.OnBpSelectionRequestByUser -= ProcessBpSelectionServerRpc;
            }
        }
    }
}