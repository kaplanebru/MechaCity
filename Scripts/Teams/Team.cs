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
            
            AllTowers.Towers.ForEach(t =>
            {
                if (t.ConstantData.StartTeam == Data.TeamType)
                {
                    Data.Towers.Add(t.Data);
                    t.Setup(Data.TeamTowerData);
                }
            });
        }
        
        
        void SetGrid()
        {
            Data.Grid.Initialize(Data.Towers);
        }

        public void TakeTowerFromRival(TowerData tower)
        {
            print("old team: " + tower.TeamTowerData.TeamType);
            Data.Towers.Add(tower);
            AllTowers.GetTower(tower.UniqID).SetTeam(Data.TeamTowerData);
            print("new Team: " + tower.TeamTowerData.TeamType);
        }

        public void RemoveTower(TowerData tower)
        {
            Data.Towers.Remove(tower);
        }
    }
}