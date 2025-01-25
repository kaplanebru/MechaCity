using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Clicks;
using Network;
using Enums;
using Testing;

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
        public PersonaType PersonaType { get; set; } //bu seçimle geliyor aslında
        public int Funds = 10; //bu da eşit gelecek zaten
        public PlayerRunData RunData { get; set; } //bu da oynadıkça belrleniyor
    }

    public class Player : NetworkBehaviour
    {
        public PlayerData Data = new();

        [SerializeField] private NetworkVariable<TeamType> ActiveTeam = new(TeamType.None,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server); //,NetworkVariableWritePermission.Owner

        public GameEndState gameEndState = GameEndState.GameStarted;
        public TurnNetworkHandler turnNetworkHandlerPrefab;

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                ActiveTeam.OnValueChanged += OnActiveTeamChanged;
                NetworkEventbus.UserEvents.OnActiveTeamSetBegin += RequestActiveTeamChangeServerRpc;
                NetworkEventbus.UserEvents.OnGameEnds += GameEndServerRpc;
                NetworkEventbus.UserEvents.OnPersonaSelectedByUser += SetPersonaType;
            }

            NetworkEventbus.ServerEvents.OnPlayerSpawned?.Invoke(this, OwnerClientId);
        }

        private void SetPersonaType(PersonaType type) //not: save alınırken tutmak için
        {
            Data.PersonaType = type;
            NetworkEventbus.ServerEvents.OnPlayerPersonaSet?.Invoke(type);
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

        public void Setup(TeamType teamType, string playerName)
        {
            Data.TeamType = teamType;
            if (IsOwner)
            {
                NetworkEventbus.UIEvents.OnPlayerSet?.Invoke(playerName, teamType);
                SpawnTurnNetworkServerRpc(); //önce team belirlensin
            }
        }

        #region Input Settings

        Ray RayFromMouse() => Camera.main.ScreenPointToRay(Input.mousePosition);

        private Coroutine inputRoutine;

        private void EnableInput(bool enable)
        {
            if (!MultiplayerSetter.IsMultiplayerOn)
            {
                NetworkEventbus.UIEvents.OnTurnButtonsListenerActivationRequest?.Invoke(true);
                inputRoutine ??= StartCoroutine(nameof(InputRoutine)); //input kısmı için sadece
                return;
            }

            if (enable)
                inputRoutine ??= StartCoroutine(nameof(InputRoutine));
            else
            {
                StopCoroutine(nameof(InputRoutine));
                inputRoutine = null;
            }
        }

        IEnumerator InputRoutine()
        {
            while (true)
            {
                //(IsOwner && Input.GetMouseButtonDown(0)) /
                if (Input.GetMouseButtonDown(0)) //iki tarafın da owner playerı tıklayabiliyor demek bu
                {
                    if (Physics.Raycast(RayFromMouse(), out RaycastHit hit, Mathf.Infinity,
                            LayerMask.GetMask("Clickable")))
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
            NetworkEventbus.InputEvents.OnObjectClicked?.Invoke(new object[] {actorID}); //
            //print(towerId);
        }

        #endregion

        #region Activity Status
        
        //dikkat: team1 ile başlanırsa team change tetiklenmez + team2yi de dinliyorsa ondan gelen klon cyclic mesajı da alır
        [ServerRpc(RequireOwnership = true)]
        private void RequestActiveTeamChangeServerRpc(TeamType teamType)
        {
            // if (!IsOwner)
            // {
            //     Debug.Log("not owner");
            // }

            //Debug.Log("listens team switch event " + ActiveTeam.Value);
            ActiveTeam.Value = teamType;
        }

        private void OnActiveTeamChanged(TeamType previousvalue, TeamType newvalue)
        {
            //if(!IsOwner) return;
            //Debug.Log("client active team: " + ActiveTeam.Value + " player team type: " + Data.TeamType);

            if (ActiveTeam.Value == Data.TeamType)
                ApplyActiveTeamSettings();
            else
                ApplyPassiveTeamSettings();
            
            //if(!IsOwner) return;
            Debug.Log("set active team ui");
            NetworkEventbus.UIEvents.OnActiveTeamSet?.Invoke(ActiveTeam.Value);
        }


        private void ApplyActiveTeamSettings()
        {
            EnableInput(true);
            NetworkEventbus.UIEvents.OnBPCardsActivationRequest?.Invoke(true);
            NetworkEventbus.UIEvents.OnTurnButtonsListenerActivationRequest?.Invoke(true);
            
            
            

            Debug.Log("active team settings applied");
        }

        private void ApplyPassiveTeamSettings()
        {
            EnableInput(false);
            if (!MultiplayerSetter.IsMultiplayerOn) return; //for testing

            NetworkEventbus.UIEvents.OnBPCardsActivationRequest?.Invoke(false);
            NetworkEventbus.UIEvents.OnTurnButtonsListenerActivationRequest?.Invoke(false);

            Debug.Log("passive team settings applied");
        }


        // private void SetActiveTeam(TeamType teamType)
        // {
        //     if (!IsOwner) return;
        //     ActiveTeam.Value = teamType;
        //     Debug.Log("active team: " + ActiveTeam.Value);
        //
        //     if (ActiveTeam.Value == Data.TeamType)
        //         ApplyActiveTeamSettings();
        //     else
        //         ApplyPassiveTeamSettings();
        // }

        #endregion

        #region Win-Fail Conditions

        [ServerRpc]
        private void GameEndServerRpc(TeamType loserTeamType)
        {
            Debug.Log("game end server rpc"); //cyclic mi bak
            ClientRpcParams clientRpcParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] {OwnerClientId}
                }
            };

            if (loserTeamType == Data.TeamType)
                LoseClientRpc(clientRpcParams); //todo: check - client rpc params'ın kullanılmayışı sorun olur mu?
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
                ActiveTeam.OnValueChanged -= OnActiveTeamChanged;
                NetworkEventbus.UserEvents.OnActiveTeamSetBegin -= RequestActiveTeamChangeServerRpc;
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