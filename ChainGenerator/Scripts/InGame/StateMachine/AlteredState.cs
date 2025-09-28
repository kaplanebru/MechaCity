using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEngine;

namespace ChainInGame
{
    public class AlteredState : BaseMovingChainState
    {
        public AlteredState(ChainMover chainMover) : base(chainMover) { }
        public override void EnterState()
        {
            StopMotion();
        }

        public override void StartMotion()
        {
            if (ChainMover._cogAmount > 1)
            {
                ChainMover.StartCoroutine(ChainMover.MoveRoutine());
                ExitState();
            }
        }

        public override void StopMotion()
        {
            if (ChainMover._cogAmount > 1)
            {
                ChainMover.StopCoroutine(ChainMover.MoveRoutine());
                ChainMover.StopLinkRoutines();
            }
        }
        

        public override void ExitState()
        {
            ChainMover.SwitchState(ChainMover.OngoingState);//pauseu falselamis olur
        }
    }
}

