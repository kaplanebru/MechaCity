using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        print(OwnerClientId);
        Eventbus.NetworkEvents.OnPlayerSpawned?.Invoke(this, OwnerClientId);
    }

    public void Setup()
    {
        
    }
}