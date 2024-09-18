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
        
        public void SetTowers(int[] newTowers)
        {
            Towers = newTowers;
        }
        
        public void TowerSelected(params object[] args)
        {
            Debug.Log("tower selected by stadard");
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

            //selectedTower.Mover.ChangeHeightPhysically(selectedTower.Height += riseStep, true);
            selectedTower.UpdateHeight(riseStep);

            foreach (var tower in SafeGroup)
            {
                //tower.Mover.ChangeHeightPhysically(tower.Height -= step, false);
                selectedTower.UpdateHeight(-step);
            }
        }

        void Fall(TowerData selectedTower, int step)
        {
            if (selectedTower.AvailableHeight >= step)
            {
                //selectedTower.Mover.ChangeHeightPhysically(selectedTower.Height -= step, false);
                selectedTower.UpdateHeight(-step);
                
                var randomTower = GetRandomOtherTower(selectedTower.UniqID);
                //randomTower.Mover.ChangeHeightPhysically(randomTower.Height += step, true);
                randomTower.UpdateHeight(step);
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

                if (tower.AvailableHeight >= step) //todo eşit sonradan eklend,
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
