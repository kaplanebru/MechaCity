using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


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
        
        private void Start()
        {
            CreateTowers();
        }

        private void OnEnable()
        {
            Eventbus.CombatEvents.OnLink += SetLinkedTowers;
            Eventbus.CombatEvents.OnUnlink += ResetLinkedTowers;
        }

        private void ResetLinkedTowers(List<int> towerIds)
        {
            foreach (var id in towerIds)
            {
                var tower = GetTower(id);
                tower.Data.floor.RestoreHeight();
                tower.StopRiseFallRoutine();
            }
        }

        private void OnDisable()
        {
            Eventbus.CombatEvents.OnLink -= SetLinkedTowers;
            Eventbus.CombatEvents.OnUnlink -= ResetLinkedTowers;

        }


        private void SetLinkedTowers(List<int> towerIds)
        {
            foreach (var id in towerIds)
            {
                var tower = GetTower(id);
                tower.Data.clickHandler.EnableSelection();
                tower.Data.floor.DecreaseHeight();
                tower.StartRiseFallRoutine();
            }
        }

     
        

        void CreateTowers()
        {
            InstantiateLevelPrefab();
            ReceiveTowers();
            ReceiveTowerData();

            LinkingTowers(_towerDatas);

            GeneralEventbus.OnTowersCreated?.Invoke();
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
            _towerDatas.ForEach(t=>t.ColorHandler.ToOriginalColor());
        }

        public static void EnableClickability()
        {
            _towers.ForEach(t=>t.EnableSelection());
        }

        public static void DisableClickability()
        {
            _towers.ForEach(t=>t.DisableSelection());
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