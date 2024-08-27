using System;
using System.Collections.Generic;
using Enums;
using GameUI;
using Network;
using Towers;
using UnityEngine;
using Random = UnityEngine.Random;

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
        
        private List<TowerData> safeGroup = new ();

        public override void SubscribeToConstantEvents()
        {
            Eventbus.LinkEvents.OnFloorsOpened += LinkTowers;

        }
        
        public override void Subscribe()
        {
            NetworkEventbus.InputEvents.OnObjectClicked += TowerSelected;
            Eventbus.LinkEvents.OnLinkStateBegin?.Invoke();
        }
        
        public override void ProcessPreviousStateTransferData(BaseTurnTransferData data) //(params object[] args)
        {
            TransferData.Towers = data.Towers;
            Eventbus.LinkEvents.OnLinkLoading?.Invoke(TransferData.Towers);
        }
        
        private void LinkTowers()
        {
            AllTowers.DisableClickability();
            Eventbus.LinkEvents.OnLinkingTowers?.Invoke(TransferData.Towers);
            MediatorEventbus.ChainLinkEvents.OnLinkedTowers?.Invoke(TransferData.Towers.ToArray());
        }


        private void TowerSelected(params object[] args)
        {
            UIEventbus.OnButtonCall?.Invoke(true); //todo: temp

            int towerID = (int) args[0];
            RiseAndFall(AllTowers.GetData(towerID), 1);
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }
        
        void RiseAndFall(TowerData selectedTower, int step)
        {
            int riseStep = GetRiseHeight(selectedTower, step);
            if (riseStep == 0)
            {
                FallAndRise(selectedTower, step);
                return;
            }

            selectedTower.Mover.ChangeHeight(selectedTower.Height += riseStep, true);

            foreach (var tower in safeGroup)
            {
                tower.Mover.ChangeHeight(tower.Height -= step, false);
            }
        }

        void FallAndRise(TowerData selectedTower, int step)
        {
            if (selectedTower.Height > step)
            {
                selectedTower.Mover.ChangeHeight(selectedTower.Height -= step, false);
                
                var randomTower = GetRandomOtherTower(selectedTower.UniqID);
                randomTower.Mover.ChangeHeight(randomTower.Height += step, true);
            }
            else
            {
                Debug.Log("Not enough resource to lift that tower!");
            }
        }
        int GetRiseHeight(TowerData selectedTower, int step)
        {
            safeGroup.Clear();
            foreach (var towerID in TransferData.Towers)
            {
                if (towerID == selectedTower.UniqID)
                    continue;

                var tower = AllTowers.GetData(towerID);

                if (tower.AvailableHeight > step)
                {
                    safeGroup.Add(tower);
                }
            }

            return safeGroup.Count * step;
        }
        TowerData GetRandomOtherTower(int selectedTowerId)
        {
            int randomId;
            
            do
            {
               var index = Random.Range(0, TransferData.Towers.Count);
               randomId = TransferData.Towers[index];
            } 
            while (randomId == selectedTowerId);

            return AllTowers.GetData(randomId);
        }
        
        public override void Unsubscribe()
        {
            Eventbus.LinkEvents.OnUnlink?.Invoke(TransferData.Towers);
            MediatorEventbus.ChainLinkEvents.OnLinkBroken?.Invoke();
            NetworkEventbus.InputEvents.OnObjectClicked -= TowerSelected;
            AllTowers.EnableClickability();
        }

        public override void UnsubscribeFromConstantEvents()
        {
            Eventbus.LinkEvents.OnFloorsOpened -= LinkTowers;
        }
    }
}