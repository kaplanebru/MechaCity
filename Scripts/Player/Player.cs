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

    public List<Tower> AllTowers = new();
    //public TurnNetworkHandler turnNetworkHandler;
}

public class Player : NetworkBehaviour
{
    public PlayerData Data = new();

    public override void OnNetworkSpawn()
    {
        Eventbus.NetworkRequestEvents.OnPlayerSpawned?.Invoke(this, OwnerClientId);
    }

    public void Setup(TeamType teamType, List<Tower> allTowers)
    {
        Data.TeamType = teamType;
        Data.AllTowers = allTowers;
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
            if (clickable.teamType != Data.TeamType) return; //if (clickable.clickableObject.Data.TeamTowerData.TeamType != Data.TeamType) return;
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
        var towerObj = Data.AllTowers[towerId];
        Eventbus.InputEvents.OnObjectClicked?.Invoke(new object[] {towerObj}); //Data.Team.Data.Towers[towerId]
    }

    #region SpawnTurnNetworkServerRpc

    // [ServerRpc(RequireOwnership = false)]
    // void SpawnTurnNetworkServerRpc(ServerRpcParams serverRpcParams = default)
    // {
    //     if (!IsOwner) return;
    //     var clientId = serverRpcParams.Receive.SenderClientId;
    //     // print("sender client id: " + clientId);
    //     var turnNetwork = Instantiate(Data.turnNetworkHandler);
    //     turnNetwork.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
    // }

    #endregion
}


#region Serializing TowerNetworkData

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

#endregion
