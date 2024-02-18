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
        public List<int> TowerGroup = new();
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
            TransferData.TowerGroup = incomingData.SelectionGroup;
        }

        public override void StartState() {}
    
        private void TowerSelected(params object[] args)
        {
            int selectedTowerUniqID = (int)args[0];
            var towerID = TransferData.TowerGroup.FirstOrDefault(t => t == selectedTowerUniqID);

            if (!TransferData.TowerGroup.Contains(towerID)) return;
            RiseAndFall(AllTowers.GetTower(towerID), 1, true);
        }
    
        void RiseAndFall(Tower selectedTower, float amount, bool rise)
        {
            foreach (var towerID in TransferData.TowerGroup)
            {
                if (towerID == selectedTower.Data.UniqID)
                {
                    selectedTower.towerParts.ChangeHeight(selectedTower.Data.Height += amount);
                }
                else
                {
                    var otherTower = AllTowers.GetTower(towerID);
                    otherTower.towerParts.ChangeHeight(otherTower.Data.Height -= amount / (TransferData.TowerGroup.Count - 1));
                }
            }
            
        }

        public override void ResetPreviousTurnData()
        {
            TransferData.TowerGroup.Clear();
        }

        public override void Unsubscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked -= TowerSelected;
        }
    }
}
