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

    
        public override void Subscribe()
        {
            AllTowers.ResetTowerSelectionColors();
            BpEventbus.SettingEvents.OnCurrentBpSet += GetBpSelector; //permanent de olabilir
        }
        
        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
            incomingData = data;
            TransferData.Towers = data.Towers;
        }
        
        private void GetBpSelector(SelectionType selectionType, int maxSelectionAmount)
        {
            bpSelector = selectors[selectionType];
            if(bpSelector==null) return;
            
            bpSelector.Subscribe();
            SetBlocking();
            bpSelector.SetMaxTowers(maxSelectionAmount);
            bpSelector.StartTowers(new List<int>());
            //TODO: bp towers için resetlenen bir list tutulabilir
        }
        
        void SetBlocking() 
        {
            IBlockable blockable = (IBlockable) bpSelector;
            if(blockable == null) return;
            ((IBlockable) bpSelector).TryBlock(Teams);
        }
        
        public override void SendOutsideSelectedElements()
        {
            NetworkEventbus.TriggerEvents.OnBpExecutionRequestByUser?.Invoke(bpSelector.Towers.ToArray());
           // NetworkEventbus.TriggerEvents.OnStateChangeRequestByUser?.Invoke(incomingData.StateType);
        }

        public override void Unsubscribe()
        {
            BpEventbus.SettingEvents.OnCurrentBpSet -= GetBpSelector;
            bpSelector.Unsubscribe();
            incomingData.RestorePreviousSelectionColors();
        }

        
    }

}
