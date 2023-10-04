using Data;
using DataModels;
using Grid;
using Towers;

namespace Turn
{
    public class MatchHelper : BaseTurnHelper
    {
        public TowersDataHolder towerDatas;
        private void OnEnable()
        {
            Eventbus.CombatEvents.OnTowerGridDetection += HandleDeadTower;
        }

        private void HandleDeadTower(TowerGridRelationModel deadTowerGridModel)
        {
            var deadTower = deadTowerGridModel.Tower;
            var linkedTowers = deadTower.Data.LinkedTowerIDs;


            for (var i = linkedTowers.Count - 1; i >= 0; i--)
            {
                RematchDetachedTowers(deadTowerGridModel, AllTowers.GetTower(linkedTowers[i]));
                RemoveLink(deadTower, AllTowers.GetTower(linkedTowers[i]));
            }

            SwitchSides(deadTower);
            Eventbus.CombatEvents.OnMatchesRestored?.Invoke();
        }

        void RematchDetachedTowers(TowerGridRelationModel deadTowerGridModel, Tower detachedTower)
        {
            int deadTowerSlotId = deadTowerGridModel.Tower.Data.SlotId;

            for (int i = 1; i < GameGrid.SlotAmount - 1; i++)
            {
                int linkCounter = 0;

                linkCounter += CheckSlotForLink(deadTowerSlotId - i, deadTowerGridModel.Grid, detachedTower);
                linkCounter += CheckSlotForLink(deadTowerSlotId + i, deadTowerGridModel.Grid, detachedTower);

                if (linkCounter > 0) break;
            }
        }

        int CheckSlotForLink(int number, GameGrid grid, Tower detachedTower)
        {
            if (number is >= 0 and < GameGrid.SlotAmount)
            {
                var slot = grid.Slots[number];
                
                if (slot.Tower.Data.TeamTowerData.TeamType ==
                    detachedTower.Data.TeamTowerData.TeamType) //bug fix: karşıdaki tower aynı team'dense pas
                    return 0;

                LinkTowers(slot.Tower.Data, detachedTower.Data);
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

        void RemoveLink(Tower deadTower, Tower otherTower)
        {
            deadTower.Data.LinkedTowerIDs.Remove(otherTower.Data.UniqID);
            otherTower.Data.LinkedTowerIDs.Remove(deadTower.Data.UniqID);
        }

        void SwitchSides(Tower deadTower)
        {
            Eventbus.TeamEvents.OnTeamChange?.Invoke(deadTower);
        }

        private void OnDisable()
        {
            Eventbus.CombatEvents.OnTowerGridDetection -= HandleDeadTower;
        }

        //TODO: STAR VE ONDİSABLE'a event listener eklenmişse düzelt. Unsubscireda da olabilir
    }
}