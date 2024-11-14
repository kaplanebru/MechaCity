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

        public override void SubscribeToConstantEvents()
        {
            BpEventbus.SelectionEvents.OnCurrentBpSet += GetBpSelector; 
            BpEventbus.StateEvents.OnIntruderExecutionAttempt += SendSelections;
        }

        public override void Subscribe()
        {
            AllTowers.ResetTowerColors();
            //BpEventbus.SelectionEvents.OnCurrentBpSet += GetBpSelector;
        }

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
            incomingData = data;
            TransferData.Actors = data.Actors;
        }

        private void GetBpSelector(SelectionType selectionType)
        {
            Debug.Log(" GET SELECTOR");
            if (selectionType != SelectionType.None)
                bpSelector = SelectionReferences.Instance.GetSelector(selectionType);

            else
            {
                //SendSelections(); //TODO: eski seçilmiş dataya burdan dolayı ihtiyaç duyulabilir, burdakiler gönderilir, nasılsa değişen bir selection olmayacak
                //TODO: direct execution by system
                BpEventbus.OnDirectBpExecution?.Invoke(null);
                BpEventbus.StateEvents.OnDirectStateChangeFromIntruder?.Invoke(true); //buraya networkten geliniyor, butona tıklama yok, networkten değişmemesi lazım
                return;
            }
            
            //AllTowers.ResetTowerColors();

            bpSelector.Subscribe();
            bpSelector.SetTeamsAndBlock(TeamsByTurn);
            bpSelector.RestartWithNewTowers(); //ContinueTowers(new List<int>());
        }

        private void SendSelections()
        {
            BpEventbus.OnSendingSelectionsForExecution?.Invoke(bpSelector?.SendAllTowers().ToArray()); 
        }

        public override void Unsubscribe()
        {
            //BpEventbus.SelectionEvents.OnCurrentBpSet -= GetBpSelector;
            if (bpSelector != null) //TODO: CHECK MİGHT CAUSE TROUBLE FOR MP
                bpSelector.Unsubscribe();
        }

        public override void UnsubscribeFromConstantEvents()
        {
            BpEventbus.SelectionEvents.OnCurrentBpSet -= GetBpSelector;
            BpEventbus.StateEvents.OnIntruderExecutionAttempt -= SendSelections;
        }
    }
}