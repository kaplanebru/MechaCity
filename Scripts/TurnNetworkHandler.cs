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
    public TeamType ownerTeamType;

    public override void OnNetworkSpawn()
    {
       
        currentTeamType.OnValueChanged += RequestTeamSwitch; //INFO: for any client that's connected to. Any owner mı yani?
        turnHandlerType.OnValueChanged += CompleteActionSetup;
        
        if (IsOwner)
        {
            Eventbus.TurnEvents.OnTurnEnded += RequestNewTurnServerRpc;

            Eventbus.NetworkTriggerEvents.OnCompleteActionRequestByUser += CompleteActionSetupServerRpc;
            Eventbus.NetworkTriggerEvents.OnTeamSwitchSetup += TeamTypeUpdateServerRpc;
            Eventbus.TurnEvents.OnTurnStarted += SendTurnButtonsSetup; //TurnUISetupServerRpc; //TurnUISetupClientRpc; 
        }

    }
    
    private void SendTurnButtonsSetup()
    {
        if (currentTeamType.Value == ownerTeamType)
            Eventbus.NetworkRequestEvents.OnTurnButtonsShiftRequest?.Invoke();
    }
    

    #region Team Switch

    [ServerRpc]
    private void TeamTypeUpdateServerRpc(TeamType newTeamType)
    {
        currentTeamType.Value = newTeamType;
    }
    private void RequestTeamSwitch(TeamType previousvalue, TeamType newvalue)
    {
        Eventbus.NetworkRequestEvents.TeamSwitchRequest?.Invoke(newvalue);
    }

    #endregion
    
    #region Complete Turn Handle

    [ServerRpc]
    void CompleteActionSetupServerRpc(TurnHandlerType lastType)
    {
        int nextType = ((int)lastType + 1) % Enum.GetValues(typeof(TurnHandlerType)).Length;
        turnHandlerType.Value =  (TurnHandlerType) nextType;
    }
    
    [ServerRpc]
    void RequestNewTurnServerRpc()
    {
        turnHandlerType.Value = TurnHandlerType.Selection;
    }
    
    private void CompleteActionSetup(TurnHandlerType previousvalue, TurnHandlerType newvalue)
    {
        //print("complete action 2 : " + newvalue);

        if(newvalue != TurnHandlerType.Selection)
            Eventbus.NetworkRequestEvents.OnCompleteActionRequest?.Invoke();
        else
            Eventbus.NetworkRequestEvents.OnNewTurnRequest?.Invoke();
    }
    
    
    #endregion
    
    
    public override void OnNetworkDespawn()
    {
        currentTeamType.OnValueChanged -= RequestTeamSwitch;
        turnHandlerType.OnValueChanged -= CompleteActionSetup;
        if (IsOwner)
        {
            Eventbus.TurnEvents.OnTurnEnded -= RequestNewTurnServerRpc;
            
            Eventbus.NetworkTriggerEvents.OnCompleteActionRequestByUser -= CompleteActionSetupServerRpc;
            Eventbus.NetworkTriggerEvents.OnTeamSwitchSetup -= TeamTypeUpdateServerRpc;
            Eventbus.TurnEvents.OnTurnStarted -= SendTurnButtonsSetup;

        }
        
    }
}