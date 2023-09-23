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
    //public NetworkVariable<TeamType> currentTeamType = new NetworkVariable<TeamType>(TeamType.Team1);
    public TeamType ownerTeamType;
    

    public override void OnNetworkSpawn()
    {
       
        turnHandlerType.OnValueChanged += CompleteActionSetup;  //owner ve clone'u değişir
        
        if (IsOwner)
        {
            Eventbus.TurnEvents.OnTurnEnding += RequestNewTurnServerRpc;

            Eventbus.NetworkTriggerEvents.OnCompleteActionRequestByUser += CompleteActionSetupServerRpc;
            Eventbus.TurnEvents.OnTurnStarted += TurnButtonsSetup;
        }

    }
    
    private void Start()
    {
        ownerTeamType = NetworkManager.LocalClient.PlayerObject.GetComponent<Player>().Data.TeamType;
    }

    void TurnButtonsSetup(TeamType currentTeamType)
    {
        print("owner team type: " + ownerTeamType + " currentTeamType: " + currentTeamType);
        if (currentTeamType == ownerTeamType)
            Eventbus.NetworkRequestEvents.OnTurnButtonsShiftRequest?.Invoke();
    }
    
    
    
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
        print("complete action : " + newvalue);

        if(newvalue != TurnHandlerType.Selection)
            Eventbus.NetworkRequestEvents.OnCompleteActionRequest?.Invoke();
        else
            Eventbus.NetworkRequestEvents.OnNewTurnRequest?.Invoke();
    }
    
    
    #endregion
    
    
    public override void OnNetworkDespawn()
    {
        turnHandlerType.OnValueChanged -= CompleteActionSetup;
        if (IsOwner)
        {
            Eventbus.TurnEvents.OnTurnEnding -= RequestNewTurnServerRpc;
            Eventbus.NetworkTriggerEvents.OnCompleteActionRequestByUser -= CompleteActionSetupServerRpc;
            Eventbus.TurnEvents.OnTurnStarted -= TurnButtonsSetup;

        }
        
    }
}