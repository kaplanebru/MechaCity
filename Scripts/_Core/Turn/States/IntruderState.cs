using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using DataModels;
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
    public class IntruderState: BaseTurnState, ITransferDataHolder<IntruderTransferData>
    {
        public override TurnStateType StateType { get; } = TurnStateType.Intruder;
        public override int StateId { get; set; }
        public IntruderTransferData TransferData { get; private set; } = new();
        
        public BpSelector bpSelector; // = new ();

        private BaseTurnTransferData incomingData;
        
        
        public override void Register()
        {
            bpSelector = new BpSelector();
        }

        public override void Subscribe()
        {
            AllTowers.ResetTowerSelectionColors();
            bpSelector.Subscribe();
        }

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
            incomingData = data;
            TransferData.Towers = data.Towers;
            
            bpSelector.StartTowers(new List<int>()); //TODO : BU towerları bir şekilde manager'a göndermesi lazım
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
