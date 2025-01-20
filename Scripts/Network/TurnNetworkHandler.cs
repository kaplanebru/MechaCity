using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Enums;
using Unity.Netcode;
using PlayerNetwork;
using Testing;
using UnityEngine;


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
        //public NetworkVariable<TurnStateType> turnStateType = new(TurnStateType.Exit);
        //_player = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>();


        public override void OnNetworkSpawn()
        {
            //turnStateType.OnValueChanged += ChangeStateRequestByServer; //owner ve clone'u değişir
            
            if (IsOwner)
            {
                NetworkEventbus.UserEvents.OnStateChangeRequestByUser += StateChangeBeginServerRpc;
                NetworkEventbus.UserEvents.OnSetCurrentBpRequestByUser += ProcessBpSelectionServerRpc;
                NetworkEventbus.UserEvents.OnBpExecutionRequestByUser += BpExecutionBeginServerRpc;
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
            NetworkEventbus.ServerEvents.OnBpSelectionByClientRpc?.Invoke(bpType, level);
           
        }


        #region Complete Turn Handle

        [ServerRpc]
        void StateChangeBeginServerRpc(TurnStateType nextType) 
        {
            StateChangeBeginClientRpc(nextType);
            //turnStateType.Value = nextType;
        }

        [ClientRpc]
        void StateChangeBeginClientRpc(TurnStateType nextType)
        {
            NetworkEventbus.ServerEvents.OnStateChangeRequestByClientRpc?.Invoke(nextType);
        }

        // private void ChangeStateRequestByServer(TurnStateType previousvalue, TurnStateType newvalue)
        // {
        //     Debug.Log("old newtork value: " + turnStateType.Value + " new network value: " + nextType);
        //     NetworkEventbus.ServerEvents.OnStateChangeRequestByServer?.Invoke(newvalue);
        //     Debug.Log("new state by system: " + newvalue); //test: 2 clientta da kaç kez çağrıldığına bak: on value changed'in ownerda olmayışını kontrol etmek amaç
        // }

        #endregion


        public override void OnNetworkDespawn()
        {
            //turnStateType.OnValueChanged -= ChangeStateRequestByServer;
            if (IsOwner)
            {
                NetworkEventbus.UserEvents.OnStateChangeRequestByUser -= StateChangeBeginServerRpc;
                NetworkEventbus.UserEvents.OnBpExecutionRequestByUser -= BpExecutionBeginServerRpc;
                NetworkEventbus.UserEvents.OnSetCurrentBpRequestByUser -= ProcessBpSelectionServerRpc;
            }
        }
    }
}