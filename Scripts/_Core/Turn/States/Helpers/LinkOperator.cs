using System.Collections;
using System.Collections.Generic;
using Actor;
using Enums;
using GameUI;
using Towers;
using UnityEngine;

namespace Turn
{
    public class LinkOperator: ILinkOperator
    {
        private int[] Towers { get; set; }
        public List<Tower> SafeGroup { get; set; } = new();
        
        public void SetTowers(uint[] actors)
        {
            List<int> newTowers = new();
            foreach (var actorID in actors)
            {
                newTowers.AddRange(ActorDB.GetTowerIDs(actorID));
            }
            Towers = newTowers.ToArray();
        }
        
        public void TowerSelected(params object[] args)
        {
            UIEventbus.OnApplyPossibility?.Invoke(true); //todo: temp

            uint actorID = (uint) args[0];
            var actor = ActorDB.Registry[actorID];
            var towerID = ActorDB.GetTowerIDs(actorID)[0];  
            
            Rise(actor.TowerHeightCouples[0], 1);
            MediatorEventbus.ChainMotionEvents.OnRising?.Invoke();
        }
        
        void Rise(TowerHeightCouple selectedTowerData, int step)
        {
            int riseStep = GetRiseHeight(selectedTowerData.Numeric, step);
            if (riseStep == 0)
            {
                Fall(selectedTowerData, step);
                return;
            }
            
            selectedTowerData.UpdateHeight(riseStep);

            foreach (var safeTower in SafeGroup)
            {
                safeTower.UpdateHeight(-step);
            }
        }

        void Fall(TowerHeightCouple selectedTowerData, int step)
        {
            if (selectedTowerData.Numeric.AvailableHeight >= step)
            {
                selectedTowerData.UpdateHeight(-step);
                
                var randomTower = GetRandomOtherTower(selectedTowerData.Numeric.UniqID);
                
                randomTower.UpdateHeight(step);
            }
            else
            {
                Debug.Log("Not enough resource to lift that tower!");
            }
        }
        int GetRiseHeight(TowerNumericData selectedTower, int step)
        {
            SafeGroup.Clear();
            foreach (var towerID in Towers)
            {
                if (towerID == selectedTower.UniqID)
                    continue;

                var tower = AllTowers.GetTower(towerID);

                if (tower.NumericData.AvailableHeight >= step) //todo eşit sonradan eklend,
                {
                    SafeGroup.Add(tower);
                }
            }

            return SafeGroup.Count * step;
        }
        private Tower GetRandomOtherTower(int selectedTowerId)
        {
            int randomId;
            
            do
            {
                var index = Random.Range(0, Towers.Length);
                randomId = Towers[index];
            } 
            while (randomId == selectedTowerId);

            return AllTowers.GetTower(randomId);
        }
    }

}
