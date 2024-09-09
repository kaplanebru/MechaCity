using System.Collections;
using System.Collections.Generic;
using GameUI;
using Towers;
using UnityEngine;

namespace Turn
{
    public class LinkOperator
    {
        protected int[] towers;
        protected List<TowerData> safeGroup = new ();

        public void GetTowers(int[] newTowers)
        {
            towers = newTowers;
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

            foreach (var tower in safeGroup)
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
            safeGroup.Clear();
            foreach (var towerID in towers)
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
        protected TowerData GetRandomOtherTower(int selectedTowerId)
        {
            int randomId;
            
            do
            {
                var index = Random.Range(0, towers.Length);
                randomId = towers[index];
            } 
            while (randomId == selectedTowerId);

            return AllTowers.GetData(randomId);
        }
    }

}
