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
            public static Action<TurnStateType> OnCompleteStateRequestByServer;
            public static Action<BpType> OnBpSelectionByServer;
            public static Action OnNewTurnRequest;
            public static Action OnTurnButtonsShiftRequest;

            public static Action<GameEndState> OnGameEndScreenRequest;
        }

        public static class TriggerEvents
        {
            public static Action<TurnStateType> OnStateChangeRequestByUser;
            public static Action<BpType> OnBpSelectionRequestByUser;
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
         
            public static Action<BpType> OnBpInstallBegin;
            public static Action<BpType> OnBpInstalled;

           
            public static Action OnStateIntrusionEnd;
        }
    }
}