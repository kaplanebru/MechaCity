using System;
using System.Collections;
using System.Collections.Generic;
using Chain;
using UnityEngine;

namespace ChainInGame
{
    public abstract class BaseMovingChainState
    {
        protected ChainMover ChainMover;

        public BaseMovingChainState(ChainMover chainMover)
        {
            ChainMover = chainMover;
        }
        public abstract void EnterState();
        public abstract void StartMotion();
        public abstract void StopMotion();
        
        public abstract void ExitState();

    }
    
    

}
