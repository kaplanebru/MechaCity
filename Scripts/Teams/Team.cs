using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Enums;
using Towers;
using UnityEngine;



namespace Teams
{
    public static class TowerEvents
    {
        public static Action<List<Tower>> OnTowersCreated;
    }

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
            //AssignTowers();
            SetGrid();
            SetTowers();
        }
        

       
        // void AssignTowers()
        // {
        //     foreach (var towerID in Data.TowerIds)
        //     {
        //         _towers[towerID].Data = Data.TowerDatas[towerID];
        //     }
        // }

        void SetGrid()
        {
            Data.Grid.Initialize(Data.TowerIds);
        }

        void SetTowers()
        {
            
            for (int i = 0; i < Data.TowerIds.Count; i++)
            {
                // int uniqIdAdditive = Data.TeamType == TeamType.Team1 ? 0 : _towers.Count;
                // var tower = _towers[i];
                // tower.Data.UniqID = i + uniqIdAdditive;
                //Data.TowerIds.Add(tower.Data.UniqID);
                
                var tower = AllTowers.Towers[Data.TowerIds[i]];
                tower.Data.SlotId = i;
                tower.Setup(Data.TeamTowerData);
            }
        }

        public void TakeTowerFromRival(int towerID)
        {
            Data.TowerIds.Add(towerID);
            AllTowers.Towers[towerID].SetTeam(Data.TeamTowerData);
        }

        public void RemoveTower(int towerID)
        {
            Data.TowerIds.Remove(towerID);
        }

        public void LinkFirstMatches(Team rivalTeam) //Temporary
        {
            for (int i = 0; i < Data.TowerIds.Count; i++)
            {
                Data.towerDatas.GetTowerData(Data.TowerIds[i]).LinkedTowerIDs.Add(rivalTeam.Data.TowerIds[i]);
            }
        }
    }
}