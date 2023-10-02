using System;
using Data;
using Unity.Netcode;
using UnityEngine;
using Clicks;
using Network;

namespace PlayerNetwork
{
    [Serializable]
    public class PlayerData
    {
        public TeamType TeamType;
        public GameEndState GameEndState = GameEndState.GameStarted;
        public TurnNetworkHandler turnNetworkHandlerPrefab;
    }

    public class Player : NetworkBehaviour
    {
        public PlayerData Data = new();

        public override void OnNetworkSpawn()
        {
            if (IsOwner) Eventbus.NetworkTriggerEvents.OnGameEnds += GameEndServerRpc;
            Eventbus.NetworkRequestEvents.OnPlayerSpawned?.Invoke(this, OwnerClientId);
        }
        
        #region SpawnTurnNetworkServerRpc
        
        [ServerRpc(RequireOwnership = false)]
        void SpawnTurnNetworkServerRpc(ServerRpcParams serverRpcParams = default)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;
            // print("sender client id: " + clientId);
            var turnNetworkHandler = Instantiate(Data.turnNetworkHandlerPrefab);
            turnNetworkHandler.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        }
        #endregion
        
        public void Setup(TeamType teamType)
        {
            Data.TeamType = teamType;
            if(IsOwner) SpawnTurnNetworkServerRpc(); //önce team belirlensin
        }

        Ray RayFromMouse() => Camera.main.ScreenPointToRay(Input.mousePosition);

        private void Update()
        {
            if (IsOwner && Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(RayFromMouse(), out RaycastHit hit))
                {
                    ClickOnTower(hit);
                }
            }
        }

        void ClickOnTower(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out Clickable clickable))
            {
                if (clickable.teamType != Data.TeamType) return;
                SendTowerIdToServerRpc(clickable.id);
            }
        }

        [ServerRpc]
        void SendTowerIdToServerRpc(int towerId)
        {
            AdjustTowerClientRpc(towerId);
        }

        [ClientRpc]
        void AdjustTowerClientRpc(int towerId) //burda da hem owner hem klonu dahil clienttaki
        {
            Eventbus.InputEvents.OnObjectClicked?.Invoke(new object[] {towerId}); //
            print(towerId);
        }
        
        #region WinFailConditions

        [ServerRpc]
        private void GameEndServerRpc(TeamType loserTeamType)
        {
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] {OwnerClientId}
                }
            };

            if (loserTeamType == Data.TeamType)
                LoseClientRpc(clientRpcParams);
            else
                WinClientRpc(clientRpcParams);
        }

        [ClientRpc]
        void WinClientRpc(ClientRpcParams clientRpcParams)
        {
            if (!IsOwner) return;
            Data.GameEndState = GameEndState.Win;
            Eventbus.NetworkRequestEvents.OnGameEndScreenRequest?.Invoke(Data.GameEndState);
        }

        [ClientRpc]
        void LoseClientRpc(ClientRpcParams clientRpcParams)
        {
            if (!IsOwner) return;
            Data.GameEndState = GameEndState.Lose;
            Eventbus.NetworkRequestEvents.OnGameEndScreenRequest?.Invoke(Data.GameEndState);
        }

        #endregion


        public override void OnNetworkDespawn()
        {
            if (IsOwner)
                Eventbus.NetworkTriggerEvents.OnGameEnds -= GameEndServerRpc;
        }
    }
    

    #region Serializing TowerNetworkData

    // public struct TowerNetworkData : INetworkSerializable, IEquatable<TowerNetworkData>
    // {
    //     public int Id;
    //     public int Height;
    //
    //     public TowerNetworkData(int id, int height)
    //     {
    //         Id = id;
    //         Height = height;
    //     }
    //
    //     public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    //     {
    //         serializer.SerializeValue(ref Id);
    //         serializer.SerializeValue(ref Height);
    //     }
    //
    //     public bool Equals(TowerNetworkData other)
    //     {
    //         return Id == other.Id && Height == other.Height;
    //     }
    //
    //     public override bool Equals(object obj)
    //     {
    //         return obj is TowerNetworkData other && Equals(other);
    //     }
    //
    //     public override int GetHashCode()
    //     {
    //         return HashCode.Combine(Id, Height);
    //     }
    // }

    #endregion
}