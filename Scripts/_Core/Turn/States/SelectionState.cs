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
        
        private SelectorWithBlocker<RivalBlocker> mainSelector;
        public SelectionTransferData TransferData { get; private set; } = new();

        public override TurnStateType StateType => TurnStateType.Selection;
        public override int StateId { get; set; }

        public override void Register()
        {
            mainSelector = new();
        }
        

        public override void Subscribe()
        {
            mainSelector.Subscribe();
            mainSelector.TryBlock(Teams);
            BpEventbus.SubscriberEvents.OnSelectionIncrease += UpdateSelectionAmount;
            BpEventbus.SubscriberEvents.OnSelectionRestoration += ResetSelection;

        }

        private void UpdateSelectionAmount()
        {
            Debug.Log("update selection");
            mainSelector.SelectionTowerAmount++;
        }

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
            TransferData.Towers = data.Towers;
            mainSelector.StartTowers(TransferData.Towers);
        }

        public override void Unsubscribe()
        {
            TransferData.Towers = mainSelector.Towers;
            //BpEventbus.SubscriberEvents.OnSelectionRestoration -= RestoreSelection;
            mainSelector.Unsubscribe();
        }

        public void ResetSelection()
        {
            Debug.Log("reset selection");
            mainSelector.ResetSelector(); 
            BpEventbus.SubscriberEvents.OnSelectionIncrease -= UpdateSelectionAmount;
        }

       
    }
}