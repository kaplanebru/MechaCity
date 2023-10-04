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
            GetTeamTowers();
            SetSlotIDs();
            SetGrid();
        }

        void GetTeamTowers()
        {
            Data.Towers.Clear(); //TODO: team so olmayabilir
            
            AllTowers.Towers.ForEach(t =>
            {
                if (t.ConstantData.StartTeam == Data.TeamType)
                {
                    Data.Towers.Add(t.Data);
                    t.Setup(Data.TeamTowerData);
                }
            });
        }

        void SetSlotIDs()
        {
            for (int i = 0; i < Data.Towers.Count; i++)
            {
                Data.Towers[i].SlotId = i; //AllTowers.GetTower(tower.UniqID).Setup(Data.TeamTowerData);
            }
        }
        
        void SetGrid()
        {
            Data.Grid.Initialize(Data.Towers);
        }

        public void TakeTowerFromRival(TowerData tower)
        {
            Data.Towers.Add(tower);
            AllTowers.GetTower(tower.UniqID).SetTeam(Data.TeamTowerData);
            //tower.SetTeam(Data.TeamTowerData); TODO!!! SET TEAM
        }

        public void RemoveTower(TowerData tower)
        {
            Data.Towers.Remove(tower);
        }
    }
}