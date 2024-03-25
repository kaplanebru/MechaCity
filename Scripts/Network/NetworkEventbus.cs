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
            public static Action<TurnStateType> OnStateChangeRequestByServer;
            public static Action<BpType, int, int> OnBpSelectionByServer;
            public static Action<GameEndState> OnGameEndScreenRequest;
            public static Action<int[]> OnBpExecutionBySystem;
        }

        public static class TriggerEvents
        {
            public static Action<TurnStateType> OnStateChangeRequestByUser;
            public static Action<BpType, int, int> OnBpSelectionRequestByUser;
            public static Action<int[]> OnBpExecutionRequestByUser;
            public static Action<TeamType> OnGameEnds;
        }

        public static class InputEvents //TODO: bu map node'da nasıl handle ediliyordu
        {
            public static Action<object[]> OnObjectClicked;
        }

      

        public static class UIEvents
        {
            public static Action<bool> OnTurnButtonShiftRequest;
        }
    }
}