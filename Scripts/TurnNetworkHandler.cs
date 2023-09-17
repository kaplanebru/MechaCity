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
    public NetworkVariable<TeamType> currentTeamType = new NetworkVariable<TeamType>(TeamType.Team1);

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
           
            Eventbus.TurnEvents.OnTurnEnded += RequestNewTurnServerRpc;
            
            Eventbus.NetworkTriggerEvents.OnCompleteActionRequestByUser += CompleteActionRequestServerRpc;
            Eventbus.NetworkTriggerEvents.OnTeamSwitchSetup += CurrentTeamTypeUpdateServerRpc;

            //currentTeamType.OnValueChanged += RequestTeamSwitch; //is it going to work on both clients?
        }
    }

    

    [ServerRpc]
    private void CurrentTeamTypeUpdateServerRpc(TeamType newTeamType)
    {
        currentTeamType.Value = newTeamType;
        RequestTeamSwitchClientRpc();
    }
    
    [ClientRpc]
    private void RequestTeamSwitchClientRpc() //(TeamType previousvalue, TeamType newvalue)
    {
        Eventbus.NetworkRequestEvents.TeamSwitchRequest?.Invoke();
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
        Eventbus.NetworkRequestEvents.OnCompleteActionRequest?.Invoke();
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
        Eventbus.NetworkRequestEvents.OnNewTurnRequest?.Invoke();
    }

    #endregion
    
    public override void OnNetworkDespawn()
    {
        if (IsOwner)
        {
            
            Eventbus.TurnEvents.OnTurnEnded -= RequestNewTurnServerRpc;
            
            Eventbus.NetworkTriggerEvents.OnCompleteActionRequestByUser -= CompleteActionRequestServerRpc;
            Eventbus.NetworkTriggerEvents.OnTeamSwitchSetup -= CurrentTeamTypeUpdateServerRpc;
           // currentTeamType.OnValueChanged -= RequestTeamSwitch; 
        }
    }
}