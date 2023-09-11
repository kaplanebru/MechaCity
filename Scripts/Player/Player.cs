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
        //networkTowers.ForEach(t=>t.OnValueChanged += );
    }
    public void Setup(TeamType teamType)
    {
        Data.TeamType = teamType;
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
