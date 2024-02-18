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
    
    public class TowerGroupState : BaseTurnState, ITurnActionHandler<TowerGroupTransferData>
    {
        public TowerGroupTransferData TransferData { get; private set; }
    
        public override TurnStateType StateType => TurnStateType.TowerGroup;
        public override int StateId { get; set; }

       

        public override void Subscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked += TowerSelected;
        }

        public override void UpdateState(TurnManager turnManager)
        {
        }

        public override void ProcessIncomingData(BaseTurnTransferData data) //(params object[] args)
        {
            TransferData = new();
            var incomingData = (SelectionTransferData) data;
            TransferData.TowerGroup = incomingData.SelectionGroup;
        }

        public override void Setup()
        {
            
        }
    
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
    
        public override void Unsubscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked -= TowerSelected;
        }
    }
}
