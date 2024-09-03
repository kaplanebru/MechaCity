using System.Collections.Generic;
using DataModels;
using Enums;
using Enums.Selections;
using Network;
using UnityEngine;
using Teams;
using GameUI;


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
        
        private CombatHelper _combatHelper = new();
        private CombatPairController pairController = new();
        private TurnHelper turnHelper = new();

        private bool firstTurn = true;

       

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //todo: test
            }
        }

        private void OnEnable()
        {
            TeamEvents.OnTeamsSet += SetTurnTeams;
            NetworkEventbus.OnAllClientsSet += FirstTurn;
            
            NetworkEventbus.RequestEvents.OnStateChangeRequestByServer += ChangeStateBySystem;
            Eventbus.CombatEvents.OnCombatTerminated += EndTurn;
            
            
            UIEventbus.OnButtonCall += ShowButtonRequest; //todo: sadece state'i tutan bir kod olabilir, state'e göre action alan
            UIEventbus.OnButtonClicked += StateChangeRequestByUser;
            BpEventbus.StateEvents.OnStateChangeWithoutInteraction += StateChangeByIntruder;
            
            bpEventHandler = new BlueprintEventHandler(this);
        }


        private void ShowButtonRequest(bool enable)
        {
            UIEventbus.OnHighlightRequest?.Invoke(enable);
        }

        private void Initialize()
        {
            _stateHolder.RegisterStates();
            _stateHolder.SubscribeToConstantEvents();
            
            pairController.Subscribe();
            pairController.SetCombatPairs();
            
            turnHelper.Subscribe();
            
            ((ExitState) _stateHolder.GetStateByType(TurnStateType.Exit)).GetCombatHelper(_combatHelper);
            _combatHelper.GetElements(combatTimingData, pairController);
            
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
                SetNewState(_stateHolder.GetStateByType(TurnStateType.Selection));
            else
                NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(TurnStateType.Selection);
            
        }

        void StateChangeByIntruder()
        {
            currentState.ExecuteSelection();
            GetPreviousState();
        }
        
        private void StateChangeRequestByUser()
        {
            if (currentState.StateType == TurnStateType.Intruder) //apply yapılan yerde enum olabilir
            {
                StateChangeByIntruder();
            }
            else
            {
                GetNextState();
            }
           
        }

        public void GetNextState()
        {
            var nextType = _stateHolder.States[turnHelper.GetNextStateId(currentState.StateId)].StateType;
            NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(nextType);
            
            UIEventbus.OnStateShift?.Invoke(nextType);

        }
        public void GetPreviousState()
        {
            var previousType = previousState?.StateType ?? TurnStateType.Exit; //todo: check
            NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(previousType);
            
            UIEventbus.OnStateShift?.Invoke(previousType);
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

        private void OnDisable()
        {
            TeamEvents.OnTeamsSet -= SetTurnTeams;

            bpEventHandler.UnsubscribeFromBlueprintEvents();
            _stateHolder.UnsubscribeFromConstantEvents();

            NetworkEventbus.OnAllClientsSet -= FirstTurn;
            NetworkEventbus.RequestEvents.OnStateChangeRequestByServer -= ChangeStateBySystem;

            Eventbus.CombatEvents.OnCombatTerminated -= EndTurn; //TODO: check
            UIEventbus.OnButtonCall -= ShowButtonRequest;
            UIEventbus.OnButtonClicked -= StateChangeRequestByUser;
            BpEventbus.StateEvents.OnStateChangeWithoutInteraction -= StateChangeByIntruder;
            
            pairController.Unsubscribe();
            turnHelper.Unsubscribe();
        }

       
    }
}