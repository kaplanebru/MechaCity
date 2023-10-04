using System.Collections.Generic;
using Data;
using DataModels;
using Enums;
using Grid;
using Teams;
using Towers;

namespace Turn
{
    public class MatchHelper : BaseTurnHelper
    {
        private Dictionary<TeamType, GameGrid> _grids = new();
        private void OnEnable()
        {
            Eventbus.CombatEvents.OnTowerKilled += HandleDeadTower;
        }

        private void HandleDeadTower(TowerData deadTower)
        {
            var linkedTowers = deadTower.LinkedTowerIDs;
            
            for (var i = linkedTowers.Count - 1; i >= 0; i--)
            {
                RematchDetachedTowers(new TowerGridRelationModel(_grids[deadTower.TeamTowerData.TeamType], deadTower), AllTowers.GetData(linkedTowers[i]));
                RemoveLink(deadTower, AllTowers.GetData(linkedTowers[i]));
            }
            
            SwitchSides(deadTower);
            Eventbus.CombatEvents.OnMatchesRestored?.Invoke();
        }

        public void SetGrids(Team[] teams)
        {
            _grids.Clear();
            foreach (var team in teams)
            {
                _grids.Add(team.Data.TeamType, team.Data.Grid);
            }
        }
        

        void RematchDetachedTowers(TowerGridRelationModel deadTowerGridModel, TowerData detachedTower)
        {
            int deadTowerSlotId = deadTowerGridModel.Tower.SlotId;

            for (int i = 1; i < GameGrid.SlotAmount - 1; i++)
            {
                int linkCounter = 0;

                linkCounter += CheckSlotForLink(deadTowerSlotId - i, deadTowerGridModel.Grid, detachedTower);
                linkCounter += CheckSlotForLink(deadTowerSlotId + i, deadTowerGridModel.Grid, detachedTower);

                if (linkCounter > 0) break;
            }
        }

        int CheckSlotForLink(int number, GameGrid grid, TowerData detachedTower)
        {
            if (number is >= 0 and < GameGrid.SlotAmount)
            {
                var slot = grid.Slots[number];
                
                if (slot.Tower.TeamTowerData.TeamType ==
                    detachedTower.TeamTowerData.TeamType) //bug fix: karşıdaki tower aynı team'dense pas
                    return 0;

                LinkTowers(slot.Tower, detachedTower);
                return 1;
            }

            return 0;
        }

        void LinkTowers(TowerData tower1, TowerData tower2)
        {
            if (!tower1.LinkedTowerIDs.Contains(tower2.UniqID))
                tower1.LinkedTowerIDs.Add(tower2.UniqID);

            if (!tower2.LinkedTowerIDs.Contains(tower1.UniqID)) //bug fix: hem sağı gem solu alsın diye deneme
                tower2.LinkedTowerIDs.Add(tower1.UniqID);
        }

        void RemoveLink(TowerData deadTower, TowerData otherTower)
        {
            deadTower.LinkedTowerIDs.Remove(otherTower.UniqID);
            otherTower.LinkedTowerIDs.Remove(deadTower.UniqID);
        }

        void SwitchSides(TowerData deadTower)
        {
            Eventbus.TeamEvents.OnTeamChange?.Invoke(deadTower);
        }

        private void OnDisable()
        {
            Eventbus.CombatEvents.OnTowerKilled -= HandleDeadTower;
        }

        //TODO: STAR VE ONDİSABLE'a event listener eklenmişse düzelt. Unsubscireda da olabilir
    }
}