using System.Collections.Generic;
using DataModels;
using Enums;
using Enums.Selections;
using Network;
using UnityEngine;
using Teams;
using GameUI;
using Testing;


namespace Turn
{
    public class TurnManager : MonoBehaviour ////NetworkBehaviour
    {
        public TurnSubscriber Subscriber;
        public static int TurnTracker => _turnTracker; //no setter
        public CombatTimingData combatTimingData;

        private static int _turnTracker = 0;

        private BaseTurnState currentState;
        private BaseTurnState previousState;

        internal TurnStateHolder StateHolder = new();
        internal BlueprintEventHandler BpEventHandler = new ();
        internal CombatPairController PairController = new();
        internal TurnHelper TurnHelper = new();
        private CombatOperator combatOperator = new();

        private bool firstTurn = true;

        private void OnEnable()
        {
            Subscriber = new(this);
            Subscriber.Subscribe();
            
            combatOperator.SetElements(combatTimingData, PairController);
        }

        internal void HighlightButtonRequest(bool enable)
        {
            UIEventbus.OnHighlightRequest?.Invoke(enable);
        }

        internal void SendCombatPairs(bool isReversed)
        {
            Eventbus.CombatEvents.OnSendingCombatPairs?.Invoke(isReversed);
        }

        internal Team SendTeam(TeamState teamState)
        {
            return TurnHelper.TeamsByTurn[teamState]; //TeamState.CurrentTeam
        }

        private void Initialize()
        {
            SendCombatPairs(false); //false todo, hep aynı yönde başlamasın

            StateHolder.RegisterStates();
            StateHolder.SubscribeToConstantEvents();

            ((ExitState) StateHolder.GetStateByType(TurnStateType.Exit)).SetCombatOperator(combatOperator);

            if (MultiplayerSetter.FasterCombat)
            {
                FastenTurn();
            }

            Eventbus.TowerEvents.OnTurnBegin?.Invoke(); //FIRST ACT
        }

        internal void SetTurnTeams(Team[] teams)
        {
            TurnHelper.TeamsByTurn = new Dictionary<TeamState, Team>()
            {
                {TeamState.CurrentTeam, teams[0]},
                {TeamState.RivalTeam, teams[1]},
            };
        }

        internal void FirstTurn(params object[] args)
        {
            Initialize();
            ((SelectionState) StateHolder.GetStateByType(TurnStateType.Selection)).ClearSelector(); //todo: temp

            NewTurn();
            firstTurn = false;
        }

        void NewTurn()
        {
            _turnTracker++;
            print("turn track: " + _turnTracker);
            TurnHelper.ManageInput();
            SetFirstState();

            //SelectionReferences.Instance.GetSelector(SelectionType.PlayerOnlyStd).StartWithNewTowers();
            ((SelectionState) StateHolder.GetStateByType(TurnStateType.Selection)).ResetSelector();
        }

        void SetFirstState()
        {
            if (firstTurn)
            {
                SetNewState(StateHolder.GetStateByType(TurnStateType.Selection));
                UIEventbus.OnStateShift?.Invoke(TurnStateType
                    .Selection); //todo: burdaki buton rivalda da çıkabilir, fix
            }

            else
                SendStateChangeRequest(TurnStateType.Selection);
        }

        internal void SendStateChangeRequest(TurnStateType type)
        {
            NetworkEventbus.UserEvents.OnStateChangeRequestByUser?.Invoke(type);

            if (type != TurnStateType.Intruder)
                UIEventbus.OnStateShift?.Invoke(type);
        }

        internal void StateEndByUser()
        {
            if (currentState.StateType == TurnStateType.Intruder) //apply yapılan yerde enum olabilir
            {
                IntruderExecutionAttempt();
            }
            else
            {
                GetNextState();
            }
        }

        void IntruderExecutionAttempt()
        {
            BpEventbus.StateEvents.OnIntruderExecutionAttempt?.Invoke();
            GetPreviousState();
            //TODO: IntruderExecutionRequest(nextType); => bu durumda Get previous state'te state change request 2 kez çağrılmış olabilir.
        }

        public void GetNextState()
        {
            var nextType = StateHolder.States[TurnHelper.GetNextStateId(currentState.StateId)].StateType;
            SendStateChangeRequest(nextType);
        }

        internal void GetPreviousState(bool isDirect = false)
        {
            var previousType = previousState?.StateType ?? TurnStateType.Exit; //todo: check
            if (!isDirect)
                SendStateChangeRequest(previousType);
            else
                ChangeStateBySystem(previousType);
        }

        public void ChangeStateBySystem(TurnStateType newType)
        {
            currentState?.CompleteState();
            SetNewState(StateHolder.GetStateByType(newType));
        }

        public void SetNewState(BaseTurnState newState)
        {
            previousState = currentState;
            currentState = newState;

            currentState.SetTeams(TurnHelper.TeamsByTurn);
            currentState.EnterState();
            TurnHelper.GetPreviousStateData(previousState, currentState);
        }

        public void EndTurn()
        {
            if (TurnHelper.GameEnding()) return;

            TurnStatusEvents.OnTurnEnding?.Invoke();
            TurnHelper.SwitchTeams();
            NewTurn();

            foreach (var state in StateHolder.States)
            {
                var turnData = (ITransferDataHolder<BaseTurnTransferData>) state;
                turnData.TransferData.ResetPreviousTurnData();
            }
        }

        void FastenTurn()
        {
            combatTimingData.AccelerateValues();
            //combatOperator.Fasten();
        }

        private void OnDisable()
        {
            Subscriber.Unsubscribe();
        }
    }
}