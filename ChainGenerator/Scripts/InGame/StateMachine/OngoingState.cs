using System.Collections;
using System.Collections.Generic;
using Chain;
using ChainInGame;
using UnityEngine;

public class OngoingState : BaseMovingChainState
{
    public OngoingState(ChainMover chainMover) : base(chainMover) { }
    public override void EnterState()
    {
        ChainMover.pause = false;
    }

    public override void StartMotion()
    {
        ChainMover.pause = false;
    }

    public override void StopMotion()
    {
        ChainMover.pause = true;
    }

    public override void ExitState() {}
}
