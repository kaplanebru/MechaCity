using System.Collections.Generic;
using Enums;
using Enums.Selections;
using Towers;
using UnityEngine;

namespace Turn
{
    public class IntruderTransferData : BaseTurnTransferData
    {
        public override TurnStateType StateType { get; set; } = TurnStateType.Intruder;
        public override List<uint> Actors { get; set; } = new();
    }

    public class IntruderState : BaseTurnState, ITransferDataHolder<IntruderTransferData>
    {
        public override TurnStateType StateType { get; } = TurnStateType.Intruder;
        public override int StateId { get; set; }
        public IntruderTransferData TransferData { get; private set; } = new();

        protected Selector bpSelector; // = new ();

        private BaseTurnTransferData incomingData;

        public override void Register() {}
        public override void SubscribeToConstantEvents() {}

        public override void Subscribe()
        {
            AllTowers.ResetTowerColors();
            BpEventbus.SelectionEvents.OnCurrentBpSet += GetBpSelector; //permanent de olabilir
        }

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
            incomingData = data;
            TransferData.Actors = data.Actors;
        }

        private void GetBpSelector(SelectionType selectionType)
        {
            if (selectionType != SelectionType.None)
                bpSelector = SelectionReferences.Instance.GetSelector(selectionType);

            else
            {
                TryExecuteBp();
                return;
            }

            
            Debug.Log(bpSelector);

            bpSelector.Subscribe();
            bpSelector.SetTeamsAndBlock(TeamsByTurn);

            bpSelector.RestartWithNewTowers(); //ContinueTowers(new List<int>());
            //TODO: bp towers için resetlenen bir list tutulabilir
        }

        public override void TryExecuteBp()
        {
            
            Debug.Log("try execute bp");
            BpEventbus.OnSendingSelectionsForExecution?.Invoke(
                bpSelector?.SendAllTowers().ToArray()); 
            //burda tekrar networke gitmeye gerek yok!!
        }

        public override void Unsubscribe()
        {
            BpEventbus.SelectionEvents.OnCurrentBpSet -= GetBpSelector;
            if (bpSelector != null) //TODO: CHECK MİGHT CAUSE TROUBLE FOR MP
                bpSelector.Unsubscribe();
           // incomingData.RestorePreviousSelectionColors();
        }

        public override void UnsubscribeFromConstantEvents()
        {
        }
    }
}