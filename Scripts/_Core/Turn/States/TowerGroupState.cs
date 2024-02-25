using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Network;
using Towers;

namespace Turn
{
    [Serializable]
    public class TowerGroupTransferData : BaseTurnTransferData
    {
        public override List<int> Towers { get; set; } = new();
    }
    
    public class TowerGroupState : BaseTurnState, ITurnTransferHandler<TowerGroupTransferData>
    {
        public TowerGroupTransferData TransferData { get; private set; } = new();
    
        public override TurnStateType StateType => TurnStateType.TowerGroup;
        public override int StateId { get; set; }
        
        public override void Subscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked += TowerSelected;
        }
        

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data) //(params object[] args)
        {
            var incomingData = (SelectionTransferData) data;
            TransferData.Towers = incomingData.Towers;
        }

        public override void StartState() {}
    
        private void TowerSelected(params object[] args)
        {
            int selectedTowerUniqID = (int)args[0];
            var towerID = TransferData.Towers.FirstOrDefault(t => t == selectedTowerUniqID);

            if (!TransferData.Towers.Contains(towerID)) return;
            RiseAndFall(AllTowers.GetTower(towerID), 1, true);
        }
    
        void RiseAndFall(Tower selectedTower, float amount, bool rise)
        {
            foreach (var towerID in TransferData.Towers)
            {
                if (towerID == selectedTower.Data.UniqID)
                {
                    selectedTower.towerParts.ChangeHeight(selectedTower.Data.Height += amount);
                }
                else
                {
                    var otherTower = AllTowers.GetTower(towerID);
                    otherTower.towerParts.ChangeHeight(otherTower.Data.Height -= amount / (TransferData.Towers.Count - 1));
                }
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
            NetworkEventbus.InputEvents.OnObjectClicked -= TowerSelected;
        }
    }
}
