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
       
    }
    
    

    [ServerRpc(RequireOwnership = false)]
    void TestServerRpc(ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;

        // if (IsOwner)
        //     print("isOwner");
        // else
        // {
        //     print("not owner");
        //     // if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        //     // {
        //     //     var client = NetworkManager.ConnectedClients[clientId];
        //     //     client.OwnedObjects[1].ChangeOwnership(1);
        //     // }
        // }
       
        print(clientId);
        print(OwnerClientId);
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