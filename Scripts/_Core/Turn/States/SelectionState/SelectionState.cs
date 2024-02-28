using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Enums;
using Network;
using Towers;
using GameUI;
using UnityEngine;

namespace Turn
{
    [Serializable]
    public class SelectionTransferData : BaseTurnTransferData
    {
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

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data) {}

        public override void StartState() {}


        public override void ResetPreviousTurnData()
        {
            TransferData.Towers.Clear();
        }

        public override void RestorePreviousSelectionColors()
        {
            TransferData.Towers.ForEach(s => AllTowers.GetTower(s).SelectColor());
        }

        public override void Unsubscribe()
        {
            TransferData.Towers = mainSelector.Towers;
            mainSelector.Unsubscribe();
        }
    }
}