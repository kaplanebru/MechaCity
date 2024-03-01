using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Turn
{
    public class TurnStateHolder
    {
        public BaseTurnState[] States = new BaseTurnState[3];

        public SelectionState SelectionState = new SelectionState();
        public LinkState LinkState = new LinkState();
        public CombatState CombatState = new CombatState();

        public void Setup()
        {
            States[0] = SelectionState;
            States[1] = LinkState;
            States[2] = CombatState;

            for (int i = 0; i < States.Length; i++)
            {
                States[i].StateId = i;
                States[i].Register();
            }
        }

    }
}