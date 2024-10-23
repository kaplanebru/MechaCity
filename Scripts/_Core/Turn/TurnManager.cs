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
        public static int TurnTracker => _turnTracker; //no setter
        public CombatTimingData combatTimingData;

        private static int _turnTracker = 0;

        private BaseTurnState currentState;
        private BaseTurnState previousState;
        
        private TurnStateHolder _stateHolder = new();
        private BlueprintEventHandler bpEventHandler;
        
        private CombatOperator combatOperator = new();
        private CombatPairController pairController = new();
        private TurnHelper turnHelper = new();

        private bool firstTurn = true;
        
        private void OnEnable()
        {
            TeamEvents.OnTeamsSet += SetTurnTeams;
            NetworkEventbus.OnAllClientsSet += FirstTurn;
            
            NetworkEventbus.RequestEvents.OnStateChangeRequestByServer += ChangeStateBySystem;
            Eventbus.CombatEvents.OnCombatTerminated += EndTurn;
            
            
            UIEventbus.OnApplyPossibility += HighlightButtonRequest; //todo: sadece state'i tutan bir kod olabilir, state'e göre action alan
            UIEventbus.OnButtonClicked += StateEndByUser;
            
            //BpEventbus.StateEvents.OnStateChangeRequestFromIntruder += GetPreviousState;
            BpEventbus.StateEvents.StateChangeRequestToIntruder += SendStateChangeRequest;
            
            bpEventHandler = new BlueprintEventHandler(this);
            pairController.Subscribe();
            combatOperator.SetElements(combatTimingData, pairController);

        }
        
        private void HighlightButtonRequest(bool enable)
        {
            UIEventbus.OnHighlightRequest?.Invoke(enable);
        }

        private void Initialize()
        {
            _stateHolder.RegisterStates();
            _stateHolder.SubscribeToConstantEvents();
            
            turnHelper.Subscribe();
            ((ExitState) _stateHolder.GetStateByType(TurnStateType.Exit)).SetCombatOperator(combatOperator);

            if (MultiplayerSetter.FasterCombat)
            {
                FastenTurn();
            }
            
            Eventbus.TowerEvents.OnTurnBegin?.Invoke();
            
        }
        
        void SetTurnTeams(Team[] teams)
        {
            turnHelper.TeamsByTurn = new Dictionary<TeamState, Team>()
            {
                {TeamState.CurrentTeam, teams[0]},
                {TeamState.RivalTeam, teams[1]},
            };
        }

        void FirstTurn(params object[] args)
        {
            Initialize();
            ((SelectionState) _stateHolder.GetStateByType(TurnStateType.Selection)).ClearSelector(); //todo: temp

            NewTurn();
            firstTurn = false;
        }

        void NewTurn()
        {
            _turnTracker++;
            print("turn track: " + _turnTracker);
            turnHelper.ManageInput();
            SetFirstState();
            
            //SelectionReferences.Instance.GetSelector(SelectionType.PlayerOnlyStd).StartWithNewTowers();
            ((SelectionState) _stateHolder.GetStateByType(TurnStateType.Selection)).ResetSelector();
        }

        void SetFirstState()
        {
            if (firstTurn)
            {
                SetNewState(_stateHolder.GetStateByType(TurnStateType.Selection));
                UIEventbus.OnStateShift?.Invoke(TurnStateType.Selection); //todo: burdaki buton rivalda da çıkabilir, fix
            }
            
            else
                SendStateChangeRequest(TurnStateType.Selection);
        }
        private void SendStateChangeRequest(TurnStateType type)
        {
            NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(type);
            
            if(type != TurnStateType.Intruder)
                UIEventbus.OnStateShift?.Invoke(type);
        }
        private void StateEndByUser()
        {
            Debug.Log(currentState.StateType);
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
            //currentState.TryExecuteBp();
            //TODO: IntruderExecutionRequest(nextType); => bu durumda Get previous state'te state change request 2 kez çağrılmış olabilir.
        }

        public void GetNextState()
        {
            var nextType = _stateHolder.States[turnHelper.GetNextStateId(currentState.StateId)].StateType;
            SendStateChangeRequest(nextType);
          
        }
        private void GetPreviousState()
        {
            var previousType = previousState?.StateType ?? TurnStateType.Exit; //todo: check
            SendStateChangeRequest(previousType);
        }

        public void ChangeStateBySystem(TurnStateType newType)
        {
            currentState?.CompleteState();
            SetNewState(_stateHolder.GetStateByType(newType));
        }

        public void SetNewState(BaseTurnState newState)
        {
            previousState = currentState;
            currentState = newState;

            currentState.SetTeams(turnHelper.TeamsByTurn);
            currentState.EnterState();
            turnHelper.GetPreviousStateData(previousState, currentState);
        }

        public void EndTurn()
        {
            if (turnHelper.GameEnding()) return;

            TurnStatusEvents.OnTurnEnding?.Invoke();
            turnHelper.SwitchTeams();
            NewTurn();

            foreach (var state in _stateHolder.States)
            {
                var turnData = (ITransferDataHolder<BaseTurnTransferData>) state;
                turnData.TransferData.ResetPreviousTurnData();
            }
        }
        
        void FastenTurn()
        {
            combatTimingData.AccelerateValues();
            combatOperator.Fasten();
        }

        private void OnDisable()
        {
            TeamEvents.OnTeamsSet -= SetTurnTeams;

            bpEventHandler.UnsubscribeFromBlueprintEvents();
            _stateHolder.UnsubscribeFromConstantEvents();

            NetworkEventbus.OnAllClientsSet -= FirstTurn;
            NetworkEventbus.RequestEvents.OnStateChangeRequestByServer -= ChangeStateBySystem;

            Eventbus.CombatEvents.OnCombatTerminated -= EndTurn; //TODO: check
            UIEventbus.OnApplyPossibility -= HighlightButtonRequest;
            UIEventbus.OnButtonClicked -= StateEndByUser;
            
            //BpEventbus.StateEvents.OnStateChangeRequestFromIntruder -= GetPreviousState;
            BpEventbus.StateEvents.StateChangeRequestToIntruder -= SendStateChangeRequest;
            
            pairController.Unsubscribe();
            turnHelper.Unsubscribe();
        }

       
    }
}