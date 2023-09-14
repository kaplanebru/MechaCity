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
        if (IsOwner)
        {
            Eventbus.NetworkEvents.OnActionCompleteRequestByUser += CompleteActionRequestServerRpc;
            Eventbus.TurnEvents.OnTurnEnded += RequestNewTurnServerRpc;
        }
    }
    
    #region CompleteAction

    [ServerRpc]
    void CompleteActionRequestServerRpc(TurnHandlerType lastType)
    {
        int nextType = ((int)lastType + 1) % Enum.GetValues(typeof(TurnHandlerType)).Length;
        turnHandlerType.Value =  (TurnHandlerType) nextType;
        
        CompleteActionClientRpc();
    }

    [ClientRpc]
    void CompleteActionClientRpc()
    {
        Eventbus.NetworkEvents.OnActionCompletedByUser?.Invoke();
    }

    #endregion


    #region CompleteTurn

    [ServerRpc]
    void RequestNewTurnServerRpc()
    {
        turnHandlerType.Value = TurnHandlerType.Selection;
        NewTurnClientRpc();
    }

    [ClientRpc]
    void NewTurnClientRpc()
    {
        Eventbus.NetworkEvents.OnNewTurn?.Invoke();
    }

    #endregion
    
    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            Eventbus.NetworkEvents.OnActionCompleteRequestByUser -= CompleteActionRequestServerRpc;
            Eventbus.TurnEvents.OnTurnEnded -= RequestNewTurnServerRpc;
        }
    }
}