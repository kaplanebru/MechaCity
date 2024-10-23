using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Enums;
using Unity.Netcode;
using PlayerNetwork;
using Testing;


namespace Network
{
    public class TurnStateData
    {
        public TurnStateType StateType;
        public BpType BpType;

        public TurnStateData(TurnStateType stateType, BpType bpType)
        {
            StateType = stateType;
            BpType = bpType;
        }

        public TurnStateData(TurnStateType stateType)
        {
            StateType = stateType;
        }
    }
    public class TurnNetworkHandler : NetworkBehaviour
    {
        public NetworkVariable<TurnStateType> turnStateType = new(TurnStateType.Exit);
        //_player = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>();


        public override void OnNetworkSpawn()
        {
            turnStateType.OnValueChanged += StateChangeBegin; //owner ve clone'u değişir
            

            if (IsOwner)
            {
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser += StateChangeBeginServerRpc;
                NetworkEventbus.TriggerEvents.OnSetCurrentBpRequestByUser += ProcessBpSelectionServerRpc;
                NetworkEventbus.TriggerEvents.OnBpExecutionRequestByUser += BpExecutionBeginServerRpc;
            }
        }

        //BP EXE
        [ServerRpc]
        private void BpExecutionBeginServerRpc(uint[] selectedItems)
        {
            BpExecutionBeginClientRpc(selectedItems);
        }
        
        [ClientRpc]
        private void BpExecutionBeginClientRpc(uint[] selectedItems)
        {
            NetworkEventbus.ServerEvents.OnBpExecutionRequestByServer?.Invoke(selectedItems);
        }
        //BP EXE
        

        [ServerRpc]
        private void ProcessBpSelectionServerRpc(BpType bpType, int level)
        {
            ProcessBpSelectionClientRpc(bpType, level);
        }

        [ClientRpc]
        void ProcessBpSelectionClientRpc(BpType bpType, int level)
        {
           // print("owner");  //2 ownera da 1 kez gidiyor
            NetworkEventbus.ServerEvents.OnBpSelectionByServer?.Invoke(bpType, level);
           
        }


        #region Complete Turn Handle

        [ServerRpc]
        void StateChangeBeginServerRpc(TurnStateType nextType) //(TurnStateType lastType)
        {
            turnStateType.Value = nextType;
        }

        private void StateChangeBegin(TurnStateType previousvalue, TurnStateType newvalue)
        {
            NetworkEventbus.ServerEvents.OnStateChangeRequestByServer?.Invoke(newvalue);
        }

        #endregion


        public override void OnNetworkDespawn()
        {
            turnStateType.OnValueChanged -= StateChangeBegin;
            if (IsOwner)
            {
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser -= StateChangeBeginServerRpc;
                NetworkEventbus.TriggerEvents.OnBpExecutionRequestByUser -= BpExecutionBeginServerRpc;
                NetworkEventbus.TriggerEvents.OnSetCurrentBpRequestByUser -= ProcessBpSelectionServerRpc;
            }
        }
    }
}