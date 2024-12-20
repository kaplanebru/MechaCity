using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Towers
{
    public class AllTowers 
    {
        public static int TowersCount;

        private TowerRelatedsInitializer towerRelatedsInitializer = new();
        public static List<TowerObject> Towers { get; private set; } = new();
        public static List<TowerData> TowerDatas { get; private set; } = new();

        public static List<TowerNumericData> TowerNumericDatas { get; private set; } = new();

        public static TowerObject GetTower(int id) => Towers[id];
        public static TowerData GetData(int id) => TowerDatas[id]; //todo? firstordefault? Ya da id'ye göre order ettir kesinliği için

        public static TowerNumericData GetNumericData(int id) => TowerNumericDatas[id];

        public static Vector3 GetTowerPos(int id) => Towers[id].transform.position;
        public void Subscribe()
        {
            Eventbus.LinkEvents.OnLinkingTowers += SetLinkedTowersAndStartRiseFallRoutine;
            Eventbus.LinkEvents.OnUnlink += ResetLinkedTowers;
            towerRelatedsInitializer.Subscribe();
        }
        public void ReceiveTowers(List<TowerObject> towers)
        {
            Towers = towers;
            TowersCount = Towers.Count;
            TowerDatas = Towers.Select(t => t.Data).ToList();
            TowerNumericDatas = Towers.Select(t => t.Data.NumericData).ToList();
        }
        

        private void ResetLinkedTowers(List<int> towerIds)
        {
            foreach (var id in towerIds)
            {
                var tower = GetTower(id);
                tower.StopRiseFallRoutine();
            }
        }
        
        public static IEnumerable<TowerData> GetTowerDatasByIDs(params int[] towerIDs)
        {
            return towerIDs.Select(id => GetData(id));
        }

        private void SetLinkedTowersAndStartRiseFallRoutine(List<int> towerIds)
        {
            foreach (var id in towerIds)
            {
                var tower = GetTower(id);
                tower.Data.VisualData.clickHandler.EnableSelection();
                tower.StartRiseFallRoutine();
            }
        }
        
        public static void ResetTowerColors()
        {
            TowerDatas.ForEach(t=>t.VisualData.ColorHandler.ToOriginalSelectionColor());
        }

        public static void EnableClickability()
        {
            TowerDatas.ForEach(t=>t.VisualData.EnableSelection());
        }

        public static void DisableClickability()
        {
            TowerDatas.ForEach(t=>t.VisualData.DisableSelection());
        }

        public void Unsubscribe()
        {
            Eventbus.LinkEvents.OnLinkingTowers -= SetLinkedTowersAndStartRiseFallRoutine;
            Eventbus.LinkEvents.OnUnlink -= ResetLinkedTowers;
            towerRelatedsInitializer.Unsubscribe();
        }
    }
}