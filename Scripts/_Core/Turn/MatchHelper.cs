using System.Collections.Generic;
using Enums;
using GameUI;
using Grid;
using Teams;
using Towers;

namespace Turn
{
    public class MatchHelper : BaseTurnHelper
    {
        private TowerData _deadTower;
        private List<int> _linkedTowers = new();
        private GameGrid _grid;
        
        private Dictionary<TeamType, GameGrid> _grids = new();
        private void OnEnable()
        {
            Eventbus.CombatEvents.OnTowerKilled += HandleDeadTower;
        }
        
        private void HandleDeadTower(TowerData deadTower)
        {
            // SetValues(deadTower, deadTower.LinkedTowerIDs, _grids[deadTower.TeamTowerData.TeamType]);
            //
            // for (var i = _linkedTowers.Count - 1; i >= 0; i--)
            // {
            //     var linkedTower = AllTowers.GetData(_linkedTowers[i]);
            //     RematchDetachedTowers(linkedTower);
            //     RemoveLink(linkedTower);
            // }
            
            _deadTower = deadTower;
            SwitchSides();
            // var towerObject = AllTowers.GetTower(_deadTower.UniqID);
            // _deadTower.Health = towerObject.ConstantData.StartHealth;
            // UIEventbus.OnHealthChange.Invoke(deadTower.Health, towerObject.gameObject);
            
            Eventbus.CombatEvents.OnMatchesRestored?.Invoke();
        }

        void SetValues(TowerData deadTower,List<int> linkedTowerIDs, GameGrid grid)
        {
            _deadTower = deadTower;
            _linkedTowers = linkedTowerIDs;
            _grid = grid;
        }

        public void SetGrids(Team[] teams)
        {
            _grids.Clear();
            foreach (var team in teams)
            {
                _grids.Add(team.Data.TeamType, team.Data.Grid);
            }
        }
        

        void RematchDetachedTowers(TowerData detachedTower)
        {
            int deadTowerSlotId = _deadTower.SlotId;

            for (int i = 1; i < GameGrid.SlotAmount - 1; i++)
            {
                int linkCounter = 0;

                linkCounter += CheckSlotForLink(deadTowerSlotId - i, detachedTower);
                linkCounter += CheckSlotForLink(deadTowerSlotId + i, detachedTower);

                if (linkCounter > 0) break;
            }
        }

        int CheckSlotForLink(int number, TowerData detachedTower)
        {
            if (number is >= 0 and < GameGrid.SlotAmount)
            {
                var slot = _grid.Slots[number];
                
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

        void RemoveLink(TowerData otherTower)
        {
            _deadTower.LinkedTowerIDs.Remove(otherTower.UniqID);
            otherTower.LinkedTowerIDs.Remove(_deadTower.UniqID);
        }

        void SwitchSides()
        {
            Eventbus.TeamEvents.OnTeamChange?.Invoke(_deadTower);
        }

        private void OnDisable()
        {
            Eventbus.CombatEvents.OnTowerKilled -= HandleDeadTower;
            SetValues(null, null, null);
        }

        //TODO: STAR VE ONDİSABLE'a event listener eklenmişse düzelt. Unsubscireda da olabilir
    }
}