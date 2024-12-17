using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Towers
{
    public class AllTowers 
    {
        public static int TowersCount;
        public static List<Tower> Towers { get; private set; } = new();
        public static List<TowerData> TowerDatas { get; private set; } = new();
        
        public static Tower GetTower(int id) => Towers[id];
        public static TowerData GetData(int id) => TowerDatas[id]; //todo? firstordefault? Ya da id'ye göre order ettir kesinliği için

        public static Vector3 GetTowerPos(int id) => Towers[id].transform.position;
        public void Subscribe()
        {
            Eventbus.LinkEvents.OnLinkingTowers += SetLinkedTowersAndStartRiseFallRoutine;
            Eventbus.LinkEvents.OnUnlink += ResetLinkedTowers;
        }
        public void ReceiveTowers(List<Tower> towers)
        {
            Towers = towers;
            TowersCount = Towers.Count;
            ReceiveTowerData();
        }

        private void ReceiveTowerData()
        {
            for (int i = 0; i < TowersCount; i++)
            {
                TowerDatas.Add(Towers[i].Data);
            }
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
                tower.Data.clickHandler.EnableSelection();
                tower.StartRiseFallRoutine();
            }
        }
        
        public static void ResetTowerColors()
        {
            TowerDatas.ForEach(t=>t.ColorHandler.ToOriginalSelectionColor());
        }

        public static void EnableClickability()
        {
            TowerDatas.ForEach(t=>t.EnableSelection());
        }

        public static void DisableClickability()
        {
            TowerDatas.ForEach(t=>t.DisableSelection());
        }

        public void Unsubscribe()
        {
            Eventbus.LinkEvents.OnLinkingTowers -= SetLinkedTowersAndStartRiseFallRoutine;
            Eventbus.LinkEvents.OnUnlink -= ResetLinkedTowers;
        }
    }
}