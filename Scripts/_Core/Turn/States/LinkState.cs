using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using GameUI;
using Network;
using Towers;
using Unity.VisualScripting;
using UnityEngine;

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

        public override void SubscribeToConstantEvents() {}

        public override void Subscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked += TowerSelected;
            Eventbus.StateEvents.OnLinkStateBegin?.Invoke();
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
            RiseAndFall(AllTowers.GetTower(towerID), 1);
            CommunEventbus.ChainTurnEvents.OnRising?.Invoke(1);
        }
    
        private List<TowerData> safeGroup = new List<TowerData>();

        int GetRiseHeight(Tower selectedTower, int step)
        {
            safeGroup.Clear();
            foreach (var towerID in TransferData.Towers)
            {
                if(towerID == selectedTower.Data.UniqID)
                    continue;
                
                var tower = AllTowers.GetData(towerID);

                if (tower.Height > step)
                {
                    safeGroup.Add(tower);
                }
            }

            return safeGroup.Count * step;
        }
        void RiseAndFall(Tower selectedTower, int step)
        {
            int riseStep = GetRiseHeight(selectedTower, step);
            if (riseStep == 0)
            {
                Debug.Log("Not enough resource to lift that tower!");
                return;
            }
            
            selectedTower.mover.ChangeHeight(selectedTower.Data.Height += riseStep);

            foreach (var tower in safeGroup)
            {
                var otherTower = AllTowers.GetTower(tower.UniqID);
                otherTower.mover.ChangeHeight(otherTower.Data.Height -= step);
            }
            
        }

        void FallAndRise(Tower selectedTower, float size) 
        {
            selectedTower.mover.ChangeHeight(selectedTower.Data.Height -= size);
        }
        


        public override void Unsubscribe()
        {
            CommunEventbus.ChainTurnEvents.OnLinkBroken?.Invoke();
            NetworkEventbus.InputEvents.OnObjectClicked -= TowerSelected;
            AllTowers.EnableClickability();
        }

        public override void UnsubscribeFromConstantEvents() {}
    }
}
