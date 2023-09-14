using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using Unity.Netcode;
using UnityEngine;

public class TurnNetworkHandler : NetworkBehaviour
{
    public NetworkVariable<TurnHandlerType> turnHandlerType = new(TurnHandlerType.Selection);

    public override void OnNetworkSpawn()
    {
        turnHandlerType.OnValueChanged += CompleteTurnHandler;
        
        if (IsOwner)
        {
            Eventbus.NetworkEvents.OnTurnHandlerEnding += UpdateTurnValueServerRpc;
            Eventbus.TurnEvents.OnTurnCompleted += RequestNewTurnServerRpc; //UI kısmı burdan çağrılabilir
        }
        
    }
    
    //Change turn handle values
    
    [ServerRpc(RequireOwnership = true)]
    private void UpdateTurnValueServerRpc(TurnHandlerType handlerType)
    {
        print( handlerType);
        turnHandlerType.Value = handlerType;
    }
    private void CompleteTurnHandler(TurnHandlerType previousvalue, TurnHandlerType newvalue)
    {
        Eventbus.NetworkEvents.OnTurnHandleTypeChanged?.Invoke();
    }
    
    
    //Complete Turn

    [ServerRpc]
    void RequestNewTurnServerRpc()
    {
        NewTurnClientRpc();
    }

    [ClientRpc]
    void NewTurnClientRpc()
    {
        Eventbus.NetworkEvents.OnNewTurn?.Invoke();
    }
    
  
    
   
    
    public override void OnNetworkDespawn()
    {
        turnHandlerType.OnValueChanged -= CompleteTurnHandler;
        if(IsOwner) Eventbus.NetworkEvents.OnTurnHandlerEnding -= UpdateTurnValueServerRpc;
    }

    
}