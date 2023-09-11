using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using Unity.Netcode;
using UnityEngine;


[Serializable]
public class PlayerData
{
    public TeamType TeamType;
}

// public struct TowerNetworkData : INetworkSerializable, IEquatable<TowerNetworkData>
// {
//     public int Id;
//     public int Height;
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

public class Player : NetworkBehaviour
{
    //public NetworkList<TowerNetworkData> networkTowers = new();
    public PlayerData Data = new ();

    public override void OnNetworkSpawn()
    {
        print(OwnerClientId);
        Eventbus.NetworkEvents.OnPlayerSpawned?.Invoke(this, OwnerClientId);
    }

    public void Setup(TeamType teamType)
    {
        Data.TeamType = teamType;
    }
}