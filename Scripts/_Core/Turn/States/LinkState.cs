using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using GameUI;
using Network;
using Towers;

namespace Turn
{
    [Serializable]
    public class TowerGroupTransferData : BaseTurnTransferData
    {
        public override TurnStateType StateType { get; set; } = TurnStateType.Link;
        public override List<int> Towers { get; set; } = new();
    }
    
    public class LinkState : BaseTurnState, ITransferDataHolder<TowerGroupTransferData>
    {
        public TowerGroupTransferData TransferData { get; private set; } = new();
        public override TurnStateType StateType => TurnStateType.Link;
        public override int StateId { get; set; }
        
        public override void Subscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked += TowerSelected;
        }
        

        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data) //(params object[] args)
        {
            TransferData.Towers = data.Towers;
            CommunEventbus.ChainTurnEvents.OnLinkedTowers?.Invoke(TransferData.Towers.ToArray());
            
            AllTowers.DisableClickability();
            TransferData.Towers.ForEach(t=>AllTowers.GetTower(t).clickHandler.EnableSelection());
        }
        
        private void TowerSelected(params object[] args)
        {
            UIEventbus.OnButtonCall?.Invoke(true); //todo: temp

            int towerID = (int) args[0];
            RiseAndFall(AllTowers.GetTower(towerID), 1, true);
            CommunEventbus.ChainTurnEvents.OnRising?.Invoke(1);
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
            
            //todo: tam tersi olmalı: amount * (TransferData.Towers.Count - 1) yükselmeli
        }


        public override void Unsubscribe()
        {
            CommunEventbus.ChainTurnEvents.OnLinkBroken?.Invoke();
            NetworkEventbus.InputEvents.OnObjectClicked -= TowerSelected;
            AllTowers.EnableClickability();
        }
        
    }
}
