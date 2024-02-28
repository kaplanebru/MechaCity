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
        private SelfSelector selector1;
        private SelfSelector selector2;
        private SelfSelector mainSelector;
        public SelectionTransferData TransferData { get; private set; } = new();
        public int maxTowersInGroup = 2;

        public override TurnStateType StateType => TurnStateType.Selection;
        public override int StateId { get; set; }

        public override void Register()
        {
            selector1 = new SelfSelector(Initializer.Teams[0].Data, Initializer.Teams[1].Data);
            selector2 = new SelfSelector(Initializer.Teams[1].Data, Initializer.Teams[0].Data);
        }


        public override void Subscribe()
        {
            mainSelector = Teams[TeamState.CurrentTeam] == Teams[0] ? selector1 : selector2;

            mainSelector.Subscribe();
        }

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data)
        {
        }

        public override void StartState()
        {
        }


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