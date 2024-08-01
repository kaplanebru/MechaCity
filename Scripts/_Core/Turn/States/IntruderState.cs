using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Core.Turn.Selectors;
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
        
        protected Selector<BpSelectionColor> bpSelector; // = new ();
        private Dictionary<SelectionType, Selector<BpSelectionColor>> selectors = new ();

        private BaseTurnTransferData incomingData;
        
        
        public override void Register()
        {
            selectors.Add(SelectionType.PlayerOnly, new BpSelectorWithBlocker<RivalBlocker>());
            selectors.Add(SelectionType.RivalOnly, new BpSelectorWithBlocker<PlayerBlocker>());
            selectors.Add(SelectionType.All, new Selector<BpSelectionColor>());
            selectors.Add(SelectionType.None, null);
        }

        public override void SubscribeToConstantEvents() {}
        
        public override void Subscribe()
        {
            AllTowers.ResetTowerSelectionColors();
            BpEventbus.SelectionEvents.OnCurrentBpSet += GetBpSelector; //permanent de olabilir
        }
        
        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
            incomingData = data;
            TransferData.Towers = data.Towers;
        }
        
        private void GetBpSelector(SelectionType selectionType, int maxSelectionAmount)
        {
            bpSelector = selectors[selectionType];
            
            if (selectionType == SelectionType.None)
            {
                BpEventbus.StateEvents.OnStateChangeWithoutInteraction?.Invoke();
                return;
            }
            
            bpSelector.Subscribe();
            SetBlocking();
            bpSelector.SetMaxTowers(maxSelectionAmount);
            bpSelector.StartTowers(new List<int>());
            //TODO: bp towers için resetlenen bir list tutulabilir
        }
        
        void SetBlocking() 
        {
            IBlockable blockable =  bpSelector as IBlockable;
            if(blockable == null) return;
            ((IBlockable) bpSelector).TryBlock(Teams);
        }
        
        public override void ExecuteSelection()
        {
            BpEventbus.OnBpExecution?.Invoke(bpSelector?.Towers.ToArray()); //burda tekrar networke gitmeye gerek yok!!
        }
        

        public override void Unsubscribe()
        {
            BpEventbus.SelectionEvents.OnCurrentBpSet -= GetBpSelector;
            if(bpSelector != null) //TODO: CHECK MİGHT CAUSE TROUBLE FOR MP
                bpSelector.Unsubscribe();
            incomingData.RestorePreviousSelectionColors();
        }

        public override void UnsubscribeFromConstantEvents() {}
        
    }

}
