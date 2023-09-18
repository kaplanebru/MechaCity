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
        print("spawn turn network manager");
        currentTeamType.OnValueChanged += RequestTeamSwitch; //INFO: for any client that's connected to
        turnHandlerType.OnValueChanged += CompleteActionSetup;
        if (IsOwner)
        {
            Eventbus.TurnEvents.OnTurnEnded += RequestNewTurnServerRpc;

            Eventbus.NetworkTriggerEvents.OnCompleteActionRequestByUser += CompleteActionSetupServerRpc;
            Eventbus.NetworkTriggerEvents.OnTeamSwitchSetup += TeamTypeUpdateServerRpc;
        }
    }
    

    #region Team Switch

    [ServerRpc]
    private void TeamTypeUpdateServerRpc(TeamType newTeamType)
    {
        currentTeamType.Value = newTeamType;
    }
    private void RequestTeamSwitch(TeamType previousvalue, TeamType newvalue)
    {
        Eventbus.NetworkRequestEvents.TeamSwitchRequest?.Invoke();
    }

    #endregion
    
    #region Complete Turn Handle

    [ServerRpc]
    void CompleteActionSetupServerRpc(TurnHandlerType lastType)
    {
        int nextType = ((int)lastType + 1) % Enum.GetValues(typeof(TurnHandlerType)).Length;
        turnHandlerType.Value =  (TurnHandlerType) nextType;
    }
    
    private void CompleteActionSetup(TurnHandlerType previousvalue, TurnHandlerType newvalue)
    {
        print("complete action 2 : " + newvalue);

        if(newvalue != TurnHandlerType.Selection)
            Eventbus.NetworkRequestEvents.OnCompleteActionRequest?.Invoke();
        else
            Eventbus.NetworkRequestEvents.OnNewTurnRequest?.Invoke();
    }
    
    #endregion


    #region CompleteTurn

    [ServerRpc]
    void RequestNewTurnServerRpc()
    {
        turnHandlerType.Value = TurnHandlerType.Selection;
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
        }
    }
}