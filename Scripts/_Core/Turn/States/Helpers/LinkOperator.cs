
using System.Collections.Generic;
using Actor;
using GameUI;
using Towers;
using UnityEngine;

namespace Turn
{
    public class LinkOperator: ILinkOperator
    {
        private int[] Towers { get; set; }
        public List<TowerData> SafeGroup { get; set; } = new();
        
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
            
            Rise(actor.Towers[0], 1);
           // MediatorEventbus.ChainMotionEvents.OnMotion?.Invoke();
        }
        
        void Rise(TowerData selectedTower, int step)
        {
            int riseStep = GetRiseHeight(selectedTower.NumericData, step);
            if (riseStep == 0 || (selectedTower.NumericData.Height + riseStep) > AllTowers.MaxTowerHeight) //test: devamı sonradan eklendi
            {
                Fall(selectedTower, step);
                return;
            }
            
            selectedTower.UpdateHeight(riseStep);

            foreach (var safeTower in SafeGroup)
            {
                safeTower.UpdateHeight(-step);
            }
        }

        void Fall(TowerData selectedTower, int step)
        {
            if (selectedTower.NumericData.AvailableHeight >= step)
            {
                selectedTower.UpdateHeight(-step);
                
                var randomTower = GetRandomOtherTower(selectedTower.NumericData.UniqID);
                
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

                var tower = AllTowers.GetData(towerID);

                if (tower.NumericData.AvailableHeight >= step) //todo eşit sonradan eklend,
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
