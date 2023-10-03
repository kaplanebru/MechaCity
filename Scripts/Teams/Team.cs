using System;
using System.Linq;
using Data;
using Enums;
using Towers;
using UnityEngine;


namespace Teams
{
    [Serializable]
    public class TeamConstructorData
    {
        public Transform TowersPrefab;
    }

    public class Team : MonoBehaviour //<TPlayerData>: MonoBehaviour where TPlayerData : TeamData
    {
        public TeamData Data;
        [SerializeField] TeamConstructorData ConstructorData;

        public void Initialize()
        {
            AssignTowers();
            SetGrid();
            SetTowers();
        }

        void AssignTowers()
        {
            var towersPb = Instantiate(ConstructorData.TowersPrefab, transform);
            Data.Towers = towersPb.GetComponentsInChildren<Tower>().ToList();
            for (var i = 0; i < Data.Towers.Count; i++)
            {
                Data.Towers[i].Data = Data.TowerDatas[i];
            }
            
        }

        void SetGrid()
        {
            Data.Grid.Initialize(Data.Towers);
        }

        void SetTowers()
        {
            for (int i = 0; i < Data.Towers.Count; i++)
            {
                int uniqIdAdditive = Data.TeamType == TeamType.Team1 ? 0 : Data.Towers.Count;
                var tower = Data.Towers[i];
                tower.Data.UniqID = i + uniqIdAdditive;
                //tower.clickHandler.SetClickables(tower.Data.UniqID);
                
                tower.Data.SlotId = i;
                tower.Setup(Data.TeamTowerData);
            }
        }

        public void TakeTowerFromRival(Tower tower)
        {
            Data.Towers.Add(tower);
            tower.SetTeamForTowerAndClickables(Data.TeamTowerData);
        }

        public void RemoveTower(Tower tower)
        {
            Data.Towers.Remove(tower);
        }

        public void LinkFirstMatches(Team rivalTeam) //Temporary
        {
            for (int i = 0; i < Data.Towers.Count; i++)
            {
                Data.Towers[i].Data.LinkedTowers.Add(rivalTeam.Data.Towers[i]);
            }
        }
    }
}