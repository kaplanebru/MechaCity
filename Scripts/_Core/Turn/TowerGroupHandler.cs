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
        public List<Tower> TowerGroup = new();
    }
    
    public class TowerGroupHandler : BaseTurnHandler, ITurnActionHandler<TowerGroupTransferData>
    {
        public TowerGroupTransferData TransferData { get; private set; }
    
        public override TurnHandlerType HandlerType => TurnHandlerType.TowerGroup;
    
        public override void OnHandlerEnabled()
        {
            TransferData = new();
            NetworkEventbus.InputEvents.OnObjectClicked += TowerSelected;
        }
        
        public override void ProcessIncomingData(BaseTurnTransferData data) //(params object[] args)
        {
            var incomingData = (SelectionTransferData) data;
            TransferData.TowerGroup = incomingData.SelectionGroup;
        }
    
        public override void Setup() {}
    
        private void TowerSelected(params object[] args)
        {
            int selectedTowerUniqID = (int)args[0];
            var tower = TransferData.TowerGroup.FirstOrDefault(t => t.Data.UniqID == selectedTowerUniqID);
            //var tower = args[0] as Tower;
            if (tower == null) return;
    
            if (!TransferData.TowerGroup.Contains(tower)) return;
            RiseAndFall(tower, 1, true);
        }
    
        void RiseAndFall(Tower selectedTower, float amount, bool rise)
        {
            foreach (var tower in TransferData.TowerGroup)
            {
                if (tower == selectedTower)
                    tower.towerParts.ChangeHeight(tower.Data.Height += amount);
                else
                    tower.towerParts.ChangeHeight(tower.Data.Height -= amount / (TransferData.TowerGroup.Count - 1));
            }
        }
    
        public override void Unsubscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked -= TowerSelected;
        }
    
        void ResetGroups()
        {
            TransferData.TowerGroup.Clear();
        }
    }
}
