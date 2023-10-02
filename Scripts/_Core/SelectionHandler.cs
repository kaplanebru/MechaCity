using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Towers;

namespace Core
{
    [Serializable]
    public class SelectionTransferData : BaseTurnTransferData
    {
        public List<Tower> SelectionGroup = new();
        
    }

    public class SelectionHandler : BaseTurnHandler, ITurnActionHandler<SelectionTransferData>
    {
        public SelectionTransferData TransferData { get; private set; }
        public int maxTowersInGroup = 2;

        public override TurnHandlerType HandlerType => TurnHandlerType.Selection;

        public override void OnHandlerEnabled()
        {
            TransferData = new();
            TransferData.SelectionGroup.Clear();
            Eventbus.InputEvents.OnObjectClicked += TowerPartClicked;
        }

        private void TowerPartClicked(params object[] args)
        {
            //var tower = args[0] as Tower;
            int selectedTowerUniqID = (int) args[0];

            var tower = teams["currentTeam"].Data.Towers.FirstOrDefault(t => t.Data.UniqID == selectedTowerUniqID); //uniq id ile dictionary tutulabilir
            if (tower == null) return;


            //if (tower.Data.TeamTowerData.TeamType == teams["rivalTeam"].Data.TeamTowerData.TeamType) return;
            if (SelectedTwice(tower)) return;

            if (TransferData.SelectionGroup.Count == maxTowersInGroup)
                ResetSelectionGroup();

            AddToSelection(true, tower);
        }

        public override void Setup()
        {
            ManageCompleteButton(false);
        }

        void AddToSelection(bool select, Tower newTower)
        {
            newTower.towerParts.SetColor(select
                ? teams["currentTeam"].Data.TeamTowerData.SelectedMaterial
                : teams["currentTeam"].Data.TeamTowerData.DefaultMaterial);

            if (select)
                TransferData.SelectionGroup.Add(newTower);
            else
                TransferData.SelectionGroup.Remove(newTower);

            ManageCompleteButton(TransferData.SelectionGroup.Count == maxTowersInGroup);
        }

        void ManageCompleteButton(bool enable)
        {
            Eventbus.UIEvents.OnButtonCall?.Invoke(enable);
        }


        bool SelectedTwice(Tower newTower)
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

        public override void Unsubscribe()
        {
            Eventbus.InputEvents.OnObjectClicked -= TowerPartClicked;
        }
    }
}