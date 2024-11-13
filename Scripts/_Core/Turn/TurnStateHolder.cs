using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Turn
{
    public class TurnStateHolder
    {
        public const int StateCount = 4;
        public readonly BaseTurnState[] States = new BaseTurnState[StateCount];
        private readonly Dictionary<TurnStateType, BaseTurnState> _statesByType = new();
        
        public BaseTurnState GetStateByType(TurnStateType type) => _statesByType[type];

        private SelectionState SelectionState = new SelectionState();
        private LinkState LinkState = new LinkState();
        private ExitState ExitState = new ExitState();
        private IntruderState IntruderState = new IntruderState();

        public TurnStateHolder()
        {
        }
        
        public void Setup()
        {

            _statesByType.Add(TurnStateType.Selection, SelectionState);
            _statesByType.Add(TurnStateType.Link, LinkState);
            _statesByType.Add(TurnStateType.Exit, ExitState);
            _statesByType.Add(TurnStateType.Intruder, IntruderState); 
            
            States[0] = SelectionState;
            States[1] = LinkState;
            States[2] = ExitState;
            States[3] = IntruderState;

            for (int i = 0; i < States.Length; i++)
            {
                States[i].StateId = i;
                //States[i].Register();
            }
        }

        public void RegisterStates()
        {
            foreach (var state in States)
            {
                state.Register();
            }
        }

        public void SubscribeToConstantEvents()
        {
            foreach (var state in States)
            {
                state.SubscribeToConstantEvents();
            }
        }

        public void UnsubscribeFromConstantEvents()
        {
            foreach (var state in States)
            {
                state.UnsubscribeFromConstantEvents();
            }
        }
    }
}