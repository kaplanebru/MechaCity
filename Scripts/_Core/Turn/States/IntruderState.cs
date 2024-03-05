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
        
        private BpSelector bpSelector;

        private BaseTurnTransferData incomingData;
        private Dictionary<TeamType, BpSelector> selectors = new();
        
        public override void Register()
        {
            selectors.Add(TeamType.Team1, new BpSelector(Initializer.Teams[0].Data));
            selectors.Add(TeamType.Team2, new BpSelector(Initializer.Teams[1].Data));
        }

        public override void Subscribe()
        {
            AllTowers.ResetTowerSelectionColors();
            bpSelector = selectors[Teams[TeamState.CurrentTeam].Data.TeamType];
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
