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
        public static Dictionary<int, TowerObject> Towers { get; private set; } = new();
        public static Dictionary<int, TowerData> TowerDatas { get; private set; } = new();

        public static Dictionary<int, TowerNumericData> TowerNumericDatas { get; private set; } = new();

        public static TowerObject GetTower(int id) => Towers[id];
        public static TowerData GetData(int id) => TowerDatas[id];

        public static TowerNumericData GetNumericData(int id) => TowerNumericDatas[id];

        public static Vector3 GetTowerPos(int id) => Towers[id].transform.position;
        public void Subscribe()
        {
            Eventbus.LinkEvents.OnLinkingTowers += SetLinkedTowersAndStartRiseFallRoutine;
            Eventbus.LinkEvents.OnUnlink += ResetLinkedTowers;
            towerRelatedsInitializer.Subscribe();
        }
        public void ReceiveTowers(List<TowerObject> towerObjects)
        {
            TowersCount = towerObjects.Count;
            foreach (var towerObject in towerObjects)
            {
                int id = towerObject.Data.NumericData.UniqID;
                Towers.Add(id, towerObject);
                TowerDatas.Add(id, towerObject.Data);
                TowerNumericDatas.Add(id, towerObject.Data.NumericData);
            }
            
            // Towers = towerObjects;
            // TowersCount = Towers.Count;
            // TowerDatas = Towers.Select(t => t.Data).ToList();
            // TowerNumericDatas = Towers.Select(t => t.Data.NumericData).ToList();
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
            TowerDatas.Values.ToList().ForEach(t=>t.VisualData.ColorHandler.ToOriginalSelectionColor());
        }

        public static void EnableClickability()
        {
            TowerDatas.Values.ToList().ForEach(t=>t.VisualData.EnableSelection());
        }

        public static void DisableClickability()
        {
            TowerDatas.Values.ToList().ForEach(t=>t.VisualData.DisableSelection());
        }

        public void Unsubscribe()
        {
            Eventbus.LinkEvents.OnLinkingTowers -= SetLinkedTowersAndStartRiseFallRoutine;
            Eventbus.LinkEvents.OnUnlink -= ResetLinkedTowers;
            towerRelatedsInitializer.Unsubscribe();
        }
    }
}