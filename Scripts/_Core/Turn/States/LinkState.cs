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
            RiseAndFall(AllTowers.GetTower(towerID), 1, true);
            CommunEventbus.ChainTurnEvents.OnRising?.Invoke(1);
        }
    
        void RiseAndFall(Tower selectedTower, float size, bool rise)
        {
            if(!CheckHeights(selectedTower, size)) return;

            selectedTower.towerParts.ChangeHeight(selectedTower.Data.Height += size * (TransferData.Towers.Count - 1));
            
            foreach (var tower in safeGroup)
            {
                var otherTower = AllTowers.GetTower(tower.UniqID);
                
                otherTower.towerParts.ChangeHeight(otherTower.Data.Height -= tower.AlteringSize);
            }
            
            // foreach (var towerID in TransferData.Towers)
            // {
            //     if (towerID == selectedTower.Data.UniqID)
            //     {
            //         selectedTower.towerParts.ChangeHeight(selectedTower.Data.Height += size * (TransferData.Towers.Count - 1));
            //     }
            //     else
            //     {
            //         var otherTower = AllTowers.GetTower(towerID);
            //         otherTower.towerParts.ChangeHeight(otherTower.Data.Height -= size);
            //     }
            // }
        }


        private List<TowerData> safeGroup = new List<TowerData>();
        bool CheckHeights(Tower selectedTower, float size)
        {
            safeGroup.Clear();
            foreach (var towerID in TransferData.Towers)
            {
                if(towerID == selectedTower.Data.UniqID)
                    continue;
                
                var tower = AllTowers.GetData(towerID);
                tower.AlteringSize = size;
                
                if (tower.Height > size)
                {
                    safeGroup.Add(tower);
                }
            }
            
            Empty:
            if (safeGroup.Count == 0)
            {
                Debug.Log("Not enough resource to lift that tower!");
                return false;
            }
               
            if (safeGroup.Count == TransferData.Towers.Count - 1)
                return true;

            for (var i = safeGroup.Count - 1; i >= 0; i--)
            {
                var tower = safeGroup[i];
                if (tower.Height <= size * 2)
                    safeGroup.Remove(tower);
            }

            if (safeGroup.Count == 0)
               goto Empty;
            
            safeGroup.First().AlteringSize = size * 2;
            return true;
        }


        public override void Unsubscribe()
        {
            CommunEventbus.ChainTurnEvents.OnLinkBroken?.Invoke();
            NetworkEventbus.InputEvents.OnObjectClicked -= TowerSelected;
            AllTowers.EnableClickability();
        }
        
    }
}
