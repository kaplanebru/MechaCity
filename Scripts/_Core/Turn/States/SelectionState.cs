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
        public override List<int> Towers { get; set; } = new();
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

            if (TransferData.Towers.Count == maxTowersInGroup)
                ResetSelectionGroup();

            AddToSelection(true, tower.UniqID);
        }

        

        void AddToSelection(bool select, int newTower)
        {
            AllTowers.GetTower(newTower).towerParts.SetColor(select
                ? Teams["currentTeam"].Data.TeamTowerData.SelectedMaterial
                : Teams["currentTeam"].Data.TeamTowerData.DefaultMaterial);

            if (select)
                TransferData.Towers.Add(newTower);
            else
                TransferData.Towers.Remove(newTower);

            ManageCompleteButton(TransferData.Towers.Count == maxTowersInGroup);
        }

        void ManageCompleteButton(bool enable)
        {
            UIEventbus.OnButtonCall?.Invoke(enable);
        }


        bool SelectedTwice(int newTower)
        {
            if (TransferData.Towers.Contains(newTower))
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
                AddToSelection(false, TransferData.Towers[0]);
            }
        }

        public override void ResetPreviousTurnData()
        {
            TransferData.Towers.Clear();
        }

        public override void RestorePreviousSelectionColors()
        {
            TransferData.Towers.ForEach(s=>AllTowers.GetTower(s).SelectColor());
        }

        public override void Unsubscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked -= TowerPartClicked;
        }
    }
}