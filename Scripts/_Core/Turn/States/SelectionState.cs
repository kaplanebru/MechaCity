using System;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using GameUI;

namespace Turn
{
    [Serializable]
    public class SelectionTransferData : BaseTurnTransferData
    {
        public override TurnStateType StateType { get; set; } = TurnStateType.Selection;
        public override List<int> Towers { get; set; } = new();
        
    }

    public class SelectionState : BaseTurnState, ITransferDataHolder<SelectionTransferData>
    {
        
        //private SelectorWithBlocker<RivalBlocker> mainSelector;
        private SingleTypeSelector mainSelector;
        public SelectionTransferData TransferData { get; private set; } = new();

        public override TurnStateType StateType => TurnStateType.Selection;
        public override int StateId { get; set; }

        public override void Register()
        {
            mainSelector = SelectionReferences.Instance.GetSelector(SelectionType.PlayerOnlyStd) as SingleTypeSelector; 
        }

        public override void SubscribeToConstantEvents()
        {
            BpEventbus.SubscriberEvents.OnSelectionIncrease += UpdateSelectionAmount;
            BpEventbus.SubscriberEvents.OnSelectionRestoration += ResetMaxSelection;
        }


        public override void Subscribe()
        {
            mainSelector.Subscribe();
            mainSelector.SetTeamsAndBlock(TeamsByTurn);
        }

        private void UpdateSelectionAmount()
        {
            mainSelector.IncreaseMaxTowers();
        }

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
            TransferData.Towers = data.Towers;
            mainSelector.ContinueTowers(TransferData.Towers);
        }
        
        public void ResetMaxSelection()
        {
            mainSelector.ResetMaxSelection(); 
        }

        public void ResetSelector()
        {
            mainSelector.StartWithNewTowers();
        }

        public override void Unsubscribe()
        {
            TransferData.Towers = mainSelector.SendAllTowers();
            mainSelector.Unsubscribe();
        }

        public override void UnsubscribeFromConstantEvents()
        {
            BpEventbus.SubscriberEvents.OnSelectionIncrease -= UpdateSelectionAmount;
            BpEventbus.SubscriberEvents.OnSelectionRestoration -= ResetMaxSelection;
        }

    }
}