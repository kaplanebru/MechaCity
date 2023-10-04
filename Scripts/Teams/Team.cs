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
            SetGrid();
            SetTowers();
        }

        void GetTeamTowers()
        {
            Data.Towers.Clear(); //TODO: team so olmayabilir
            AllTowers.Towers.ForEach(t =>
            {
                if(t.ConstantData.StartTeam == Data.TeamType)
                    Data.Towers.Add(t.Data);
            });
        }

        void SetGrid()
        {
            Data.Grid.Initialize(Data.Towers);
        }

        void SetTowers()
        {
            for (int i = 0; i < Data.Towers.Count; i++)
            {
                var tower = Data.Towers[i];
                tower.SlotId = i;
                AllTowers.GetTower(tower.UniqID).Setup(Data.TeamTowerData);
                //tower.Setup(Data.TeamTowerData);TODO!!! SETUP
            }
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