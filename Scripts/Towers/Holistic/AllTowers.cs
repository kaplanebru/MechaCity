using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Towers
{
    public class AllTowers : MonoBehaviour
    {
        public static int TowersCount;
        public static List<Tower> Towers { get; private set; } = new();
        public static List<TowerData> TowerDatas { get; private set; } = new();
     
        
        public static Tower GetTower(int id) => Towers[id];
        public static TowerData GetData(int id) => TowerDatas[id]; //todo? firstordefault? Ya da id'ye göre order ettir kesinliği için

        [SerializeField] Transform levelPrefab;
        Transform _level;
        
        private void Start()
        {
            CreateTowers();
        }

        private void OnEnable()
        {
            Eventbus.LinkEvents.OnLinkingTowers += SetLinkedTowersAndStartRiseFallRoutine;
            Eventbus.LinkEvents.OnUnlink += ResetLinkedTowers;
        }

        public static TowerData[] GetTowerGroup(IEnumerable<int> ids)
        {
            return TowerDatas.Where(t => ids.Contains(t.UniqID)).ToArray();
        }

        private void ResetLinkedTowers(List<int> towerIds)
        {
            foreach (var id in towerIds)
            {
                var tower = GetTower(id);
                tower.StopRiseFallRoutine();
            }
        }

        private void SetLinkedTowersAndStartRiseFallRoutine(List<int> towerIds)
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

            //LinkingTowers(_towerDatas);
            SettingNeighbours();

            GeneralEventbus.InitializerEvents.OnTowersCreated?.Invoke();
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
                TowerDatas.Add(Towers[i].Data);
            }
        }

        // public static void LinkingTowers(List<TowerData> towers) //ters de gelebilir
        // {
        //     for (var i = 0; i < TowersCount; i++)
        //     {
        //         towers[i].LinkedTowerIDs.Clear();
        //
        //         int next = towers[(i + 1) % TowersCount].UniqID; //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da
        //         towers[i].LinkedTowerIDs.Add(next);
        //         
        //        // print("index: " + (i + 1) % TowersCount + " id: " + next);
        //     }
        // }

        public void SettingNeighbours()
        {
            for (var i = 0; i < TowersCount; i++)
            {
                TowerDatas[i].NeighbourIDs.Clear();
                
                int previousID = i - 1;
                if (previousID < 0)
                    previousID = TowersCount - 1;
                int previous =  TowerDatas[previousID].UniqID;
                
                int next = TowerDatas[(i + 1) % TowersCount].UniqID;
                
                TowerDatas[i].NeighbourIDs.Add(previous);
                TowerDatas[i].NeighbourIDs.Add(next);
            }
        }

        public static void RestoreBullets()
        {
            Towers.ForEach(t => t.RestoreBullets());
        }

        public static void ResetTowerSelectionColors()
        {
            TowerDatas.ForEach(t=>t.ColorHandler.ToOriginalColor());
        }

        public static void EnableClickability()
        {
            TowerDatas.ForEach(t=>t.EnableSelection());
        }

        public static void DisableClickability()
        {
            TowerDatas.ForEach(t=>t.DisableSelection());
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
        
        private void OnDisable()
        {
            Eventbus.LinkEvents.OnLinkingTowers -= SetLinkedTowersAndStartRiseFallRoutine;
            Eventbus.LinkEvents.OnUnlink -= ResetLinkedTowers;
        }
    }
}