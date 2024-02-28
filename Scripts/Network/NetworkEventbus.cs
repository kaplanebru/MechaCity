using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using PlayerNetwork;
using UnityEngine;

namespace Network
{
    public class NetworkEventbus
    {
        public static Action<object[]> OnAllClientsSet;
        public static class RequestEvents
        {
            public static Action<Player, ulong> OnPlayerSpawned;
            public static Action OnCompleteStateRequestByServer;
            public static Action OnNewTurnRequest;
            public static Action OnTurnButtonsShiftRequest;

            public static Action<GameEndState> OnGameEndScreenRequest;
        }
        
        public static class TriggerEvents
        {
            public static Action<TurnStateType> OnCompleteStateRequestByUser;
            public static Action<TeamType> OnGameEnds;
        }
        
        public static class InputEvents //TODO: bu map node'da nasıl handle ediliyordu
        {
            public static Action<object[]> OnObjectClicked;
        }
        
        public static class TurnEvents
        {
            public static Action OnTurnEnding;
            public static Action<TeamType> OnTurnStarted;
        }
        
        public static class BlueprintEvents
        {
            public static Action<BpType> OnBpSelected;
            public static Action<object[]> OnBpReady;
            
            public static Action OnStateIntrusionAttempt;
            public static Action OnStateIntrusionEnd;
        }
    }
    
    
}

