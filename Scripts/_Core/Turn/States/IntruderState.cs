using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Enums;
using Network;
using Towers;
using Turn;
using UnityEngine;

namespace Turn
{
    public class IntruderTransferData : BaseTurnTransferData
    {
        public override TurnStateType StateType { get; set; } = TurnStateType.Intruder;
        public override List<int> Towers { get; set; } = new();
        
    }
    public class IntruderState: BaseTurnState, ITurnTransferHandler<IntruderTransferData>
    {
        public override TurnStateType StateType { get; } = TurnStateType.Intruder;
        public override int StateId { get; set; }
        public IntruderTransferData TransferData { get; private set; } = new();
        
        private BaseSelector bpSelector;

        private BaseTurnTransferData incomingData;
        
        public override void Register()
        {
            bpSelector = new BaseSelector();
        }

        public override void Subscribe()
        {
            AllTowers.ResetTowerSelectionColors();
            bpSelector.Subscribe();
        }

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
            incomingData = data;
            
            Debug.Log(incomingData.StateType + " intruder öncesi"); //buraya uğramıyor
            
            TransferData.Towers = data.Towers;
            bpSelector.GetTowers(new List<int>());
        }


        public void StopIntrusion()
        {
            NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(incomingData.StateType);
        }

        public override void Unsubscribe()
        {
            incomingData.RestorePreviousSelectionColors();

            bpSelector.Unsubscribe();
         

        }

    }

}
