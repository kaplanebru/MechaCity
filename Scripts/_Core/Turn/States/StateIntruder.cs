using System.Collections;
using System.Collections.Generic;
using Enums;
using Network;
using Turn;
using UnityEngine;

namespace Turn
{
    public class StateIntruder
    {
        private TurnStateHolder _stateHolder;
        private BaseTurnState currentState;

        public StateIntruder(TurnStateHolder stateHolder)
        {
            _stateHolder = stateHolder;
        }
        public void Activate(int i)
        {
            currentState = _stateHolder.States[i];
            currentState.Unsubscribe();
            //before play action: Show UI or not
        
            Subscribe();
            //play action
        }


        void Subscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked += Select; //bu ui'dan sonra gelebilir
        }

       

        public void Unsubscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked -= Select;
            currentState.Subscribe();
            var currentTurnData = (ITurnTransferHandler<BaseTurnTransferData>)currentState;
            currentTurnData.TransferData.RestorePreviousSelectionColors();
        }
    

        private void Select(object[] obj)
        {
        
        }


  

   
    }

}
