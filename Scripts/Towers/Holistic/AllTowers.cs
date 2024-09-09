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
        public static TowerData GetData(int id) => _towerDatas[id]; //todo? firstordefault? Ya da id'ye göre order ettir kesinliği için

        [SerializeField] Transform levelPrefab;
        Transform _level;
        
        private void Start()
        {
            CreateTowers();
        }

        private void OnEnable()
        {
            Eventbus.LinkEvents.OnLinkingTowers += SetLinkedTowers;
            Eventbus.LinkEvents.OnUnlink += ResetLinkedTowers;
        }

        public static TowerData[] GetTowerGroup(IEnumerable<int> ids)
        {
            return _towerDatas.Where(t => ids.Contains(t.UniqID)).ToArray();
        }

        private void ResetLinkedTowers(List<int> towerIds)
        {
            foreach (var id in towerIds)
            {
                var tower = GetTower(id);
                tower.StopRiseFallRoutine();
            }
        }

        private void SetLinkedTowers(List<int> towerIds)
        {
            foreach (var id in towerIds)
            {
                var tower = GetTower(id);
                tower.Data.clickHandler.EnableSelection();
                tower.StartRiseFallRoutine();
            }
        }
        
        void CreateTowers()
        {
            InstantiateLevelPrefab();
            ReceiveTowers();
            ReceiveTowerData();

            LinkingTowers(_towerDatas);
            SettingNeighbours();

            GeneralEventbus.InitializerEvents.OnTowersCreated?.Invoke();
        }

        void InstantiateLevelPrefab()
        {
            _level = Instantiate(levelPrefab, transform);
        }

        void ReceiveTowers()
        {
            _towers = _level.GetComponentsInChildren<Tower>().ToList();
           // _towers= _towers.OrderBy(t => t.Data.UniqID).ToList();

            TowersCount = _towers.Count;
        }

        void ReceiveTowerData()
        {
            for (int i = 0; i < TowersCount; i++)
            {
                _towerDatas.Add(_towers[i].Data);
            }

           // _towerDatas = _towerDatas.OrderBy(t => t.UniqID).ToList();
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

        public void SettingNeighbours()
        {
            for (var i = 0; i < TowersCount; i++)
            {
                _towerDatas[i].NeighbourIDs.Clear();
                
                int previousID = i - 1;
                if (previousID < 0)
                    previousID = TowersCount - 1;
                int previous =  _towerDatas[previousID].UniqID;
                
                int next = _towerDatas[(i + 1) % TowersCount].UniqID;
                
                _towerDatas[i].NeighbourIDs.Add(previous);
                _towerDatas[i].NeighbourIDs.Add(next);
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
            _towerDatas.ForEach(t=>t.EnableSelection());
        }

        public static void DisableClickability()
        {
            _towerDatas.ForEach(t=>t.DisableSelection());
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
        
        private void OnDisable()
        {
            Eventbus.LinkEvents.OnLinkingTowers -= SetLinkedTowers;
            Eventbus.LinkEvents.OnUnlink -= ResetLinkedTowers;
        }
    }
}