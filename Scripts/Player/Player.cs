using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (IsOwner && NetworkObjectId == 2) //temp
        {
            SendToServerRpc();
        }
    }

    [ServerRpc]
    void SendToServerRpc()
    {
        print("message received by server");
        SetClientsAsPlayersClientRpc();
    }

    [ClientRpc]
    void SetClientsAsPlayersClientRpc()
    {
        print("message sent to all clients by server");
        Eventbus.NetworkEvents.OnAllPlayersSpawned?.Invoke(NetworkManager.Singleton.ConnectedClients); //only accessible on server
    }
    


   
}