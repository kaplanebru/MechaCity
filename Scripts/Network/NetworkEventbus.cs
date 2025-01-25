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

        public static class ServerEvents
        {
            public static Action<Player, ulong> OnPlayerSpawned;
            public static Action<TurnStateType> OnStateChangeRequestByClientRpc;
            public static Action<BpType, int> OnBpSelectionByClientRpc;
            public static Action<GameEndState> OnGameEndScreenRequest;
            public static Action<uint[]> OnBpExecutionRequestByServer;
            public static Action<PersonaType> OnPlayerPersonaSet;
        }
        public static class UserEvents
        {
            public static Action<TurnStateType> OnStateChangeRequestByUser;
            public static Action<BpType, int> OnSetCurrentBpRequestByUser;
            public static Action<uint[]> OnBpExecutionRequestByUser;
            public static Action<TeamType> OnGameEnds;
            public static Action<PersonaType> OnPersonaSelectedByUser;

            public static Action<TeamType> OnActiveTeamSetBegin;
        }
        public static class InputEvents //TODO: bu map node'da nasıl handle ediliyordu
        {
            public static Action<object[]> OnObjectClicked;
        }
        public static class UIEvents
        {
            public static Action<bool> OnTurnButtonsListenerActivationRequest;
            public static Action<bool> OnBPCardsActivationRequest;
            public static Action<string, TeamType> OnPlayerSet;

            public static Action<TeamType> OnActiveTeamSet;
        }
    }
}