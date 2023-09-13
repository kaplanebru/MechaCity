using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Unity.Netcode;
using UnityEngine;


[Serializable]
public class PlayerData
{
    public TeamType TeamType;
    public Team Team; //bunun yerine sadece Towerlar da tutulabilir
    public TurnNetworkObject TurnNetworkObject;

}
public class Player : NetworkBehaviour
{
    public PlayerData Data = new ();
    public NetworkVariable<TurnHandlerType> turnHandlerType = new(TurnHandlerType.Selection);

    public override void OnNetworkSpawn()
    {
        Eventbus.NetworkEvents.OnPlayerSpawned?.Invoke(this, OwnerClientId);

        //SpawnTurnNetworkServerRpc();


        if (IsServer) //burda server rpc'ya mesaj gitmeli
            Eventbus.NetworkEvents.OnTurnHandlerEnding += ChangeHandlerValue;
        
        turnHandlerType.OnValueChanged += CompleteTurnHandler;
    }
    

    [ServerRpc(RequireOwnership = false)]
    void SpawnTurnNetworkServerRpc(ServerRpcParams serverRpcParams = default)
    {
       
        if(!IsOwner) return;
        var clientId = serverRpcParams.Receive.SenderClientId;
       // print("sender client id: " + clientId);
        var turnNetwork = Instantiate(Data.TurnNetworkObject);
        turnNetwork.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);

    }

    private void ChangeHandlerValue(TurnHandlerType handlerType)
    {
        if (!IsOwner) return;
        print("change handler value: " + handlerType);
        turnHandlerType.Value = handlerType; //TODO:complete'te değil startında gelebilir turn'ün. bÖYLECE sonsuz döngüye girmez.
    }


    private void CompleteTurnHandler(TurnHandlerType previousvalue, TurnHandlerType newvalue)
    {
        print("complete");
        Eventbus.NetworkEvents.OnPlayerTurnHandleTypeChanged?.Invoke();
    }

    public void Setup(TeamType teamType, Team team)
    {
        Data.TeamType = teamType;
        Data.Team = team;
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
            if(clickable.teamType != Data.TeamType) return;
            SendTowerIdToServerRpc(clickable.id);
        }
    }

    [ServerRpc]
    void SendTowerIdToServerRpc(int towerId)
    {
        AdjustTowerClientRpc(towerId);
    }

    [ClientRpc]
    void AdjustTowerClientRpc(int towerId)
    {
        Eventbus.InputEvents.OnObjectClicked?.Invoke(new object[] {Data.Team.Data.Towers[towerId]});
    }

    public override void OnNetworkDespawn()
    {
        turnHandlerType.OnValueChanged -= CompleteTurnHandler;
        Eventbus.NetworkEvents.OnTurnHandlerEnding -= ChangeHandlerValue;
    }
}

public struct TowerNetworkData : INetworkSerializable, IEquatable<TowerNetworkData>
{
    public int Id;
    public int Height;
    
    public TowerNetworkData(int id, int height)
    {
        Id = id;
        Height = height;
    }
    
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Id);
        serializer.SerializeValue(ref Height);
    }

    public bool Equals(TowerNetworkData other)
    {
        return Id == other.Id && Height == other.Height;
    }

    public override bool Equals(object obj)
    {
        return obj is TowerNetworkData other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Height);
    }
}
