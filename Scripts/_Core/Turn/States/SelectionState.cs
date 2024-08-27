using System;
using System.Collections.Generic;
using System.Linq;
using _Core.Turn.Selectors;
using Core;
using Enums;
using Network;
using Towers;
using GameUI;
using Unity.VisualScripting;
using UnityEngine;

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
            mainSelector = new();
            mainSelector.SetData(SelectionReferences.Instance.GetData(Selections.SelectionType.PlayerOnly));
        }

        public override void SubscribeToConstantEvents()
        {
            BpEventbus.SubscriberEvents.OnSelectionIncrease += UpdateSelectionAmount;
            BpEventbus.SubscriberEvents.OnSelectionRestoration += ResetSelection;
        }


        public override void Subscribe()
        {
            mainSelector.Subscribe();
            mainSelector.TryBlock(TeamsByTurn);
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
        
        public void ResetSelection()
        {
            mainSelector.ResetMaxSelection(); 
        }


        public override void Unsubscribe()
        {
            TransferData.Towers = mainSelector.SendAllTowers();
            mainSelector.Unsubscribe();
        }

        public override void UnsubscribeFromConstantEvents()
        {
            BpEventbus.SubscriberEvents.OnSelectionIncrease -= UpdateSelectionAmount;
            BpEventbus.SubscriberEvents.OnSelectionRestoration -= ResetSelection;
        }

    }
}