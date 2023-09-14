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
        //turnHandlerType.OnValueChanged += ChangeTurnHandlerClientRpc;
        
        if (IsOwner)
        {
            Eventbus.NetworkEvents.OnTurnHandlerBegin += UpdateTurnValueServerRpc;
            Eventbus.TurnEvents.OnTurnCompleted += RequestNewTurnServerRpc; //UI kısmı burdan çağrılabilir
            Eventbus.NetworkEvents.OnActionCompleteRequestByUser += CompleteActionRequestServerRpc;
        }
    }

    [ServerRpc]
    void CompleteActionRequestServerRpc()
    {
        CompleteActionClientRpc();
    }

    [ClientRpc]
    void CompleteActionClientRpc()
    {
        Eventbus.NetworkEvents.OnActionCompletedByUser?.Invoke();
    }
    
    [ServerRpc(RequireOwnership = true)]
    private void UpdateTurnValueServerRpc(TurnHandlerType handlerType)
    {
        print( "update value");
        turnHandlerType.Value = handlerType;
        ChangeTurnHandlerClientRpc();
    }
    
    [ClientRpc]
    private void ChangeTurnHandlerClientRpc()
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
        if(IsServer)
            turnHandlerType.Value = TurnHandlerType.Selection;
        Eventbus.NetworkEvents.OnNewTurn?.Invoke();
    }
    
  
    
   
    
    public override void OnNetworkDespawn()
    {
        //turnHandlerType.OnValueChanged -= ChangeTurnHandlerClientRpc;
        if (IsOwner)
        {
            Eventbus.NetworkEvents.OnTurnHandlerBegin -= UpdateTurnValueServerRpc;
            Eventbus.NetworkEvents.OnActionCompleteRequestByUser -= CompleteActionRequestServerRpc;
            Eventbus.TurnEvents.OnTurnCompleted -= RequestNewTurnServerRpc; 
        }
    }

    
}