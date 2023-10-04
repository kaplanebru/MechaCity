using System;
using System.Collections.Generic;
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
        
        //private Towers<Tower> _towers = new();

        public void Initialize()
        {
            AssignTowerIDs(AllTowers.TowersCount/2);
            SetGrid();
            SetTowers();
        }


        void AssignTowerIDs(int towerCount)
        {
            Data.TowerIds.Clear();
            
            int uniqIdAdditive = Data.TeamType == TeamType.Team1 ? 0 : towerCount;
            for (int i = 0; i < towerCount; i++)
            {
                Data.TowerIds.Add(i + uniqIdAdditive);
            }
        }
        
        void SetGrid()
        {
            Data.Grid.Initialize(Data.TowerIds);
        }

        void SetTowers()
        {
            for (int i = 0; i < Data.TowerIds.Count; i++)
            {
                var tower = AllTowers.GetTower(Data.TowerIds[i]);
                tower.Data.SlotId = i;
                tower.Setup(Data.TeamTowerData);
            }
        }

        public void TakeTowerFromRival(int towerID)
        {
            Data.TowerIds.Add(towerID);
            AllTowers.GetTower(towerID).SetTeam(Data.TeamTowerData);
        }

        public void RemoveTower(int towerID)
        {
            Data.TowerIds.Remove(towerID);
        }
    }
}