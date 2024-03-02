using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

namespace Turn
{
    public class TurnStateHolder
    {
        public BaseTurnState[] States = new BaseTurnState[4];

        public Dictionary<TurnStateType, BaseTurnState> StatesByType = new();

        public SelectionState SelectionState = new SelectionState();
        public LinkState LinkState = new LinkState();
        public CombatState CombatState = new CombatState();
        public IntruderState IntruderState = new IntruderState();

        public void Setup()
        {
            StatesByType.Add(TurnStateType.Selection, SelectionState);
            StatesByType.Add(TurnStateType.Link, LinkState);
            StatesByType.Add(TurnStateType.Combat, CombatState);
            StatesByType.Add(TurnStateType.Intruder, IntruderState); 
            
            States[0] = SelectionState;
            States[1] = LinkState;
            States[2] = CombatState;
            States[3] = IntruderState;

            for (int i = 0; i < States.Length; i++)
            {
                States[i].StateId = i;
                States[i].Register();
            }
        }

    }
}