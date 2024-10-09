using System;
using System.Collections.Generic;
using Enums;
using Towers;
using UnityEngine;



namespace Teams
{
    public class Team : MonoBehaviour //<TPlayerData>: MonoBehaviour where TPlayerData : TeamData
    {
        public TeamData Data;
        
        public void Initialize()
        {
            GetTeamTowers();
            SetGrid();
        }

        void GetTeamTowers()
        {
            Data.Towers.Clear(); //TODO: team so olmayabilir

            foreach (var t in AllTowers.Towers)
            {
                if (t.ConstantData.StartTeam == Data.TeamType)
                {
                    Data.Towers.Add(t.Data);
                    t.Setup(Data.TeamTowerData);
                }
            }
            
            
        }
        
        void SetGrid()
        {
            //Data.Grid.Initialize(Data.Towers);
        }

        public void TakeTowerFromRival(TowerData tower)
        {
            Data.Towers.Add(tower);

            Tower towerObj = AllTowers.GetTower(tower.UniqID);
            towerObj.SetTeam(Data.TeamTowerData);
        }

        public void RemoveTower(TowerData tower)
        {
            Data.Towers.Remove(tower);
        }
    }
    
    public static class TeamEvents
    {
        public static Action<Dictionary<TeamState, Team>> OnTeamsSent;
        public static Action OnTeamsRequest;
    }
}