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

        private static List<Tower> _towers = new();
        private static List<TowerData> _towerDatas = new();
        public static IEnumerable<Tower> Towers => _towers;
        public static IEnumerable<TowerData> TowerDatas => _towerDatas;
        public static Tower GetTower(int id) => _towers[id];
        public static TowerData GetData(int id) => _towerDatas[id];

        [SerializeField] Transform levelPrefab;
        Transform _level;

        private void OnEnable()
        {
            CreateTowers();
        }

        void CreateTowers()
        {
            InstantiateLevelPrefab();
            ReceiveTowers();
            ReceiveTowerData();

            LinkingTowers(_towerDatas);

            TowerEvents.OnTowersCreated?.Invoke();
        }

        void InstantiateLevelPrefab()
        {
            _level = Instantiate(levelPrefab, transform);
        }

        void ReceiveTowers()
        {
            _towers = _level.GetComponentsInChildren<Tower>().ToList();
            TowersCount = _towers.Count;
        }

        void ReceiveTowerData()
        {
            for (int i = 0; i < TowersCount; i++)
            {
                _towerDatas.Add(_towers[i].Data);
            }
        }

        public static void LinkingTowers(List<TowerData> towers) //ters de gelebilir
        {
            for (var i = 0; i < TowersCount; i++)
            {
                towers[i].LinkedTowerIDs.Clear();

                int next = towers[(i + 1) % TowersCount].UniqID; //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da
                towers[i].LinkedTowerIDs.Add(next);
                
               // print("index: " + (i + 1) % TowersCount + " id: " + next);
            }
        }

        public static void RestoreBullets()
        {
            _towers.ForEach(t => t.RestoreBullets());
        }

        public static void ResetTowerSelectionColors()
        {
            _towers.ForEach(t=>t.ResetColor());
        }

        public static void ResetClickability()
        {
            _towers.ForEach(t=>t.clickHandler.EnableSelection());
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            foreach (var tower in _towers)
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