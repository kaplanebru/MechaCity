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
        //AskToChangeOwnershipServerRpc();
        TestServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    void TestServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;

        if (IsOwner)
        {
            print("isOwner");
        }
        else
        {
            print("not owner");

        }
        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            var client = NetworkManager.ConnectedClients[clientId];
            client.OwnedObjects[1].ChangeOwnership(1);
        }
        print(clientId);
    }


    [ServerRpc(RequireOwnership = false)]
    void AskToChangeOwnershipServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        var client = NetworkManager.ConnectedClients[clientId];
        //print( client.OwnedObjects[1].name);
        print(client.OwnedObjects.Count);


        //if(IsOwner) return;

        if (NetworkManager.ConnectedClients.Count < 2) return;

        //  var client = NetworkManager.ConnectedClients.Values.Last();
        // print( client.OwnedObjects[0].name);
        //  client.OwnedObjects[0].ChangeOwnership(1);
    }


    void GetToKnow()
    {
        print("sa");
        if (IsOwner)
            print("owwner");
        

        if (IsServer)
            print("server");
        

        if (IsClient)
            print("client");
        
    }
}