using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Clicks;
using Network;
using Enums;

namespace PlayerNetwork
{
    public class PlayerRunData
    {
        public int Win;
        public int Fail;
        public int Draw;
    }
    
    [Serializable]
    public class PlayerData
    {
        public TeamType TeamType { get; set; } //bu teamle geliyor
        public PersonaType PersonaType; // { get; set; }//bu seçimle geliyor aslında
        public int Funds = 10; //bu da eşit gelecek zaten
        public PlayerRunData RunData { get; set; } //bu da oynadıkça belrleniyor
    }

    public class Player : NetworkBehaviour
    {
        public PlayerData Data = new();
        public GameEndState gameEndState = GameEndState.GameStarted;
        public TurnNetworkHandler turnNetworkHandlerPrefab;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                NetworkEventbus.UserEvents.OnGameEnds += GameEndServerRpc;
                NetworkEventbus.UserEvents.OnPersonaSelectedByUser += SetPersonaType;
            }
            NetworkEventbus.ServerEvents.OnPlayerSpawned?.Invoke(this, OwnerClientId);
        }

        private void SetPersonaType(PersonaType type)
        {
            Data.PersonaType = type;
            Debug.Log(type);
        }
        
        #region SpawnTurnNetworkServerRpc
        
        [ServerRpc(RequireOwnership = false)]
        void SpawnTurnNetworkServerRpc(ServerRpcParams serverRpcParams = default)
        {
            var clientId = serverRpcParams.Receive.SenderClientId;
            // print("sender client id: " + clientId);
            var turnNetworkHandler = Instantiate(turnNetworkHandlerPrefab);
            turnNetworkHandler.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        }
        #endregion
        
        public void Setup(TeamType teamType)
        {
            Data.TeamType = teamType;
            if(IsOwner) SpawnTurnNetworkServerRpc(); //önce team belirlensin
        }

        Ray RayFromMouse() => Camera.main.ScreenPointToRay(Input.mousePosition);
        
        public void EnableInput(bool enable)
        {
            if(!IsOwner) return;
            NetworkEventbus.UIEvents.OnTurnButtonShiftRequest?.Invoke(enable);
            
            if (enable)
                StartCoroutine(nameof(InputRoutine));
            else
                StopCoroutine(nameof(InputRoutine));
        }
        IEnumerator InputRoutine()
        {
            while (true)
            {
                //(IsOwner && Input.GetMouseButtonDown(0)) /
                if (Input.GetMouseButtonDown(0)) //iki tarafın da owner playerı tıklayabiliyor demek bu
                {
                    if (Physics.Raycast(RayFromMouse(), out RaycastHit hit,Mathf.Infinity, LayerMask.GetMask("Clickable")))
                    {
                        ClickOnTower(hit);
                       
                    }
                }
                yield return null;
            }
        }

        void ClickOnTower(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out Clickable clickable))
            {
               SendTowerIdToServerRpc(clickable.id);
            }
        }

        [ServerRpc]
        void SendTowerIdToServerRpc(uint actorID)
        {
            AdjustTowerClientRpc(actorID);
        }

        [ClientRpc]
        void AdjustTowerClientRpc(uint actorID) //burda da hem owner hem klonu dahil clienttaki
        {
            NetworkEventbus.InputEvents.OnObjectClicked?.Invoke(new object[] {actorID }); //
            //print(towerId);
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
            gameEndState = GameEndState.Win;
            NetworkEventbus.ServerEvents.OnGameEndScreenRequest?.Invoke(gameEndState);
        }

        [ClientRpc]
        void LoseClientRpc(ClientRpcParams clientRpcParams)
        {
            if (!IsOwner) return;
            gameEndState = GameEndState.Lose;
            NetworkEventbus.ServerEvents.OnGameEndScreenRequest?.Invoke(gameEndState);
        }

        #endregion


        public override void OnNetworkDespawn()
        {
            if (IsOwner)
            {
                NetworkEventbus.UserEvents.OnGameEnds -= GameEndServerRpc;
                NetworkEventbus.UserEvents.OnPersonaSelectedByUser -= SetPersonaType;
            }
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