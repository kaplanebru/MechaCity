using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<int> SelectionGroup = new();
        
    }

    public class SelectionState : BaseTurnState, ITurnTransferHandler<SelectionTransferData>
    {
        public SelectionTransferData TransferData { get; private set; } = new();
        public int maxTowersInGroup = 2;

        public override TurnStateType StateType => TurnStateType.Selection;
        public override int StateId { get; set; }
       
        
        public override void Subscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked += TowerPartClicked;
        }

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data) {}
        
        public override void StartState()
        {
            ManageCompleteButton(false);
        }
        
        

        private void TowerPartClicked(params object[] args)
        {
            int selectedTowerUniqID = (int) args[0];

            var tower = Teams["currentTeam"].Data.Towers.FirstOrDefault(t => t.UniqID == selectedTowerUniqID);
            if (tower == null) return;


            if (SelectedTwice(tower.UniqID)) return;

            if (TransferData.SelectionGroup.Count == maxTowersInGroup)
                ResetSelectionGroup();

            AddToSelection(true, tower.UniqID);
        }

        

        void AddToSelection(bool select, int newTower)
        {
            AllTowers.GetTower(newTower).towerParts.SetColor(select
                ? Teams["currentTeam"].Data.TeamTowerData.SelectedMaterial
                : Teams["currentTeam"].Data.TeamTowerData.DefaultMaterial);

            if (select)
                TransferData.SelectionGroup.Add(newTower);
            else
                TransferData.SelectionGroup.Remove(newTower);

            ManageCompleteButton(TransferData.SelectionGroup.Count == maxTowersInGroup);
        }

        void ManageCompleteButton(bool enable)
        {
            UIEventbus.OnButtonCall?.Invoke(enable);
        }


        bool SelectedTwice(int newTower)
        {
            if (TransferData.SelectionGroup.Contains(newTower))
            {
                AddToSelection(false, newTower);
                return true;
            }

            return false;
        }

        void ResetSelectionGroup()
        {
            for (int i = 0; i < maxTowersInGroup; i++)
            {
                AddToSelection(false, TransferData.SelectionGroup[0]);
            }
        }

        public override void ResetPreviousTurnData()
        {
            TransferData.SelectionGroup.Clear();
        }

        public override void Unsubscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked -= TowerPartClicked;
        }
    }
}