using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Unity.Netcode;
using UnityEngine;

public class TurnNetworkObject : NetworkBehaviour
{
    public NetworkVariable<TurnHandlerType> turnHandlerType = new(TurnHandlerType.Selection);

    public override void OnNetworkSpawn()
    {
        if(IsOwner) Eventbus.NetworkEvents.OnTurnHandlerEnding += UpdateTurnValueServerRpc;
        turnHandlerType.OnValueChanged += CompleteTurnHandler;
    }
    
    [ServerRpc(RequireOwnership = true)]
    private void UpdateTurnValueServerRpc(TurnHandlerType handlerType)
    {
        print("change handler value: " + handlerType);
        turnHandlerType.Value = handlerType; 
        //CompleteTurnHandlerClientRpc();
    }
    
    private void CompleteTurnHandler(TurnHandlerType previousvalue, TurnHandlerType newvalue)
    {
        Eventbus.NetworkEvents.OnPlayerTurnHandleTypeChanged?.Invoke();
        print("complete client on value change");
    }
    
    [ClientRpc]
    void CompleteTurnHandlerClientRpc()
    {
        print("complete client");
        Eventbus.NetworkEvents.OnPlayerTurnHandleTypeChanged?.Invoke();
    }
    
    public override void OnNetworkDespawn()
    {
        turnHandlerType.OnValueChanged -= CompleteTurnHandler;
        if(IsOwner) Eventbus.NetworkEvents.OnTurnHandlerEnding -= UpdateTurnValueServerRpc;
    }

    
}