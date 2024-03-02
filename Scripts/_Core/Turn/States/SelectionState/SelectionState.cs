using System;
using System.Collections.Generic;
using System.Linq;
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

    public class SelectionState : BaseTurnState, ITurnTransferHandler<SelectionTransferData>
    {
        private Dictionary<TeamType, SelfSelector> selectors = new ();
       
        private SelfSelector mainSelector;
        public SelectionTransferData TransferData { get; private set; } = new();

        public override TurnStateType StateType => TurnStateType.Selection;
        public override int StateId { get; set; }

        public override void Register()
        {
            selectors.Add(TeamType.Team1, new SelfSelector(Initializer.Teams[0].Data, Initializer.Teams[1].Data));
            selectors.Add(TeamType.Team2, new SelfSelector(Initializer.Teams[1].Data, Initializer.Teams[0].Data));
        }
        
        public override void Subscribe()
        {
            mainSelector = selectors[Teams[TeamState.CurrentTeam].Data.TeamType];
            mainSelector.Subscribe();
        }

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
            var incomingData = data;
            TransferData.Towers.Clear();
            TransferData.Towers.AddRange(incomingData.Towers);
            
            //Debug.Log(TransferData.Towers.Count);
            Debug.Log("state: " + StateType + " previous data: "+incomingData.StateType + " " + incomingData.Towers.Count);

            mainSelector.Towers.AddRange(TransferData.Towers);           
            //mainSelector.Towers = TransferData.Towers;
            
        }
        
        public override void Unsubscribe()
        {
            TransferData.Towers.Clear();
            TransferData.Towers.AddRange(mainSelector.Towers);
            mainSelector.Unsubscribe();
        }
    }
}