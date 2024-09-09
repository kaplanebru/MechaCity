using System.Collections;
using System.Collections.Generic;
using Enums;
using GameUI;
using Towers;
using UnityEngine;

namespace Turn
{
    public class LinkOperator: ILinkOperator
    {
        public LinkOperatorType Type { get; set; } = LinkOperatorType.Standard;
        public int[] Towers { get; set; }
        public List<TowerData> SafeGroup { get; set; } = new();
        
        public void GetTowers(int[] newTowers)
        {
            Towers = newTowers;
        }
        
        public virtual void TowerSelected(params object[] args)
        {
            UIEventbus.OnApplyPossibility?.Invoke(true); //todo: temp

            int towerID = (int) args[0];
            
            Rise(AllTowers.GetData(towerID), 1);
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }
        
        void Rise(TowerData selectedTower, int step)
        {
            int riseStep = GetRiseHeight(selectedTower, step);
            if (riseStep == 0)
            {
                Fall(selectedTower, step);
                return;
            }

            selectedTower.Mover.ChangeHeight(selectedTower.Height += riseStep, true);

            foreach (var tower in SafeGroup)
            {
                tower.Mover.ChangeHeight(tower.Height -= step, false);
            }
        }

        void Fall(TowerData selectedTower, int step)
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
            SafeGroup.Clear();
            foreach (var towerID in Towers)
            {
                if (towerID == selectedTower.UniqID)
                    continue;

                var tower = AllTowers.GetData(towerID);

                if (tower.AvailableHeight > step)
                {
                    SafeGroup.Add(tower);
                }
            }

            return SafeGroup.Count * step;
        }
        private TowerData GetRandomOtherTower(int selectedTowerId)
        {
            int randomId;
            
            do
            {
                var index = Random.Range(0, Towers.Length);
                randomId = Towers[index];
            } 
            while (randomId == selectedTowerId);

            return AllTowers.GetData(randomId);
        }
    }

}
