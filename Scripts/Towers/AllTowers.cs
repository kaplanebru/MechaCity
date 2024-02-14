using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TowerEvents
{
    public static Action OnTowersCreated;
}

namespace Towers
{
    public class AllTowers : MonoBehaviour
    {
        public static int TowersCount;
        public static List<Tower> Towers = new();
        public static List<TowerData> Datas = new();

        [SerializeField] Transform levelPrefab;
        Transform _level;
        


        public static Tower GetTower(int id) => Towers[id]; //Towers.FirstOrDefault(t => t.Data.UniqID == id);
        public static TowerData GetData(int id) => Datas[id]; //Towers[id].Data;//Datas[i];


        private void OnEnable()
        {
            CreateTowers();
        }

        void CreateTowers()
        {
            InstantiateLevelPrefab();
            ReceiveTowers();
            ReceiveTowerData();
            SetFirstMatches();

            TowerEvents.OnTowersCreated?.Invoke();
        }

        void InstantiateLevelPrefab()
        {
            _level = Instantiate(levelPrefab, transform);
        }

        void ReceiveTowers()
        {
            Towers = _level.GetComponentsInChildren<Tower>().ToList();
            TowersCount = Towers.Count;
        }
        void ReceiveTowerData()
        {
            for (int i = 0; i < TowersCount; i++)
            {
                Datas.Add(Towers[i].Data);
            }
        }

        void SetFirstMatches()
        {
            int dataSize = Datas.Count;
            for (int i = 0; i < dataSize; i++)
            {
                var nextIndex = (i + 1) % dataSize;
               // var prevIndex = (i - 1 + dataSize) % dataSize;
                
                Datas[i].LinkedTowerIDs.Add(nextIndex);
               // Datas[i].LinkedTowerIDs.Add(prevIndex);
            }
        }

        public static void RestoreBullets()
        {
            Towers.ForEach(t => t.RestoreBullets());
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            foreach (var tower in Towers)
            {
                foreach (var linkedTowerID in tower.Data.LinkedTowerIDs)
                {
                    if (tower == null) continue;
                    Gizmos.DrawLine(tower.transform.position, GetTower(linkedTowerID).transform.position);
                }
            }
        }
    }
}