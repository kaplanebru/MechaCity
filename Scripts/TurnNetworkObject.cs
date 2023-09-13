using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Unity.Netcode;
using UnityEngine;

public class TurnNetworkObject : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        //print("client id: " + OwnerClientId);
        if (IsClient)
        {
            //NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        }


        //client sayısının dolması gerekecek önce.


        // GetToKnow();
    }

    private void OnClientConnected(ulong clientId)
    {
        //print("on connected client id: " + OwnerClientId);
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            //Debug.Log("Client connected. You can now call ServerRpc methods.");
            //AskToChangeOwnershipServerRpc();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    void AskToChangeOwnershipServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        //if (NetworkManager.ConnectedClients.ContainsKey(1))

        // print("sender client id: " + clientId);
        // print("changing ownership");
        if (NetworkManager.ConnectedClients.Count == 2)
        {
            var client = NetworkManager.ConnectedClients.Values.Last();
            client.OwnedObjects[0].ChangeOwnership(1);
        }
        
        
        ChangeOwnershipClientRpc();
    }


    [ClientRpc]
    void ChangeOwnershipClientRpc()
    {
        if (IsServer) return;
        
        // if(!IsOwner)
        //     GetComponent<NetworkObject>().ChangeOwnership(1); //NetworkManager.Singleton.ConnectedClients.Last().Value.ClientId
        //
        // print("ownership changed");
    }


    void GetToKnow()
    {
        print("sa");
        if (IsOwner)
        {
            print("owwner");
        }

        if (IsServer)
        {
            print("server");
        }

        if (IsClient)
        {
            print("client");
        }
    }
}