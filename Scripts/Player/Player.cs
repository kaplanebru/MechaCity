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
    public Team Team;
}
public class Player : NetworkBehaviour
{
   
    //public NetworkList<TowerNetworkData> NetworkTowers;
    //NetworkList<int> towers;
    //public NetworkVariable<TowerNetworkData> tower;
    public List<NetworkVariable<int>> networkTowers;
    public PlayerData Data = new ();

    public override void OnNetworkSpawn()
    {
        Eventbus.NetworkEvents.OnPlayerSpawned?.Invoke(this, OwnerClientId);
        SetNetworkTowers();
    }
    
    public void Setup(TeamType teamType, Team team)
    {
        Data.TeamType = teamType;
        Data.Team = team;
    }

    private void Update()
    {
        if (IsOwner && Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out Clickable clickable))
                {
                    if(clickable.teamType != Data.TeamType) return;
                    SendTowerIdToServerRpc(clickable.id);
                    //team de tutulabilir
                }
            }
           
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


    void SetNetworkTowers()
    {
        //towers = new NetworkList<int> {} ;
        networkTowers = new();
        for (int i = 0; i < GameGrid.SlotAmount; i++)
        {
            var tower = new NetworkVariable<int>();
            tower.Value = i;
            networkTowers.Add(tower);
        }
    }

    void SubscribeOnValueChangedEvent(bool enable)
    {
        networkTowers.ForEach(t=>t.OnValueChanged += SendTeamEvent);
    }

    private void SendTeamEvent(int previousvalue, int newvalue) //id'yi bilmiyoruz bu durumda
    {
        throw new NotImplementedException();
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
