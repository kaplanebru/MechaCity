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
                    Data.Towers.Add(t);
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
                tower.Data.SlotId = i;
                tower.Setup(Data.TeamTowerData);
            }
        }

        public void TakeTowerFromRival(Tower tower)
        {
            Data.Towers.Add(tower);
            tower.SetTeam(Data.TeamTowerData);
        }

        public void RemoveTower(Tower tower)
        {
            Data.Towers.Remove(tower);
        }
    }
}