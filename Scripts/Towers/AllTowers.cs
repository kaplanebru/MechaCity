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

        [SerializeField] Transform[] TowersPrefab;
        [SerializeField] TowersDataHolder constantDatas;
        


        public static Tower GetTower(int id) => Towers[id]; //Towers.FirstOrDefault(t => t.Data.UniqID == id);
        public static TowerData GetData(int id) => Datas[id]; //Towers[id].Data;//Datas[i];
        

        private void OnEnable()
        {
            CreateTowers();
        }

        void CreateTowers()
        {
            InstantiateTowers();
            CreateDatas();
            AssignDatasToTowers();
            TowerEvents.OnTowersCreated?.Invoke();
        }

        void InstantiateTowers()
        {
            foreach (var prefab in TowersPrefab)
            {
                var towersPb = Instantiate(prefab, transform);
                Towers.AddRange(towersPb.GetComponentsInChildren<Tower>().ToList());
            }
            
            TowersCount = Towers.Count;
        }

        void CreateDatas()
        {
            for (int i = 0; i < TowersCount; i++)
            {
                Datas.Add(new TowerData(i));
            }
        }

        void AssignDatasToTowers()
        {
            for (int i = 0; i < TowersCount; i++)
            {
                var tower = Towers[i];
                tower.Data = Datas[i];
                tower.ConstantData = constantDatas.Datas[i];
            }
            SetFirstMatches();
        }
        
        void SetFirstMatches()
        {
            int teamTowerAmount = TowersCount / 2;
            for (var i = 0; i < teamTowerAmount; i++)
            {
                Datas[i].LinkedTowerIDs.Add(Datas[i + teamTowerAmount].UniqID);
                Datas[i + teamTowerAmount].LinkedTowerIDs.Add(Datas[i].UniqID);
            }
           
        }
        
        public static void RestoreBullets()
        {
            Towers.ForEach(t => t.RestoreBullets());
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < Towers.Count / 2; i++)
            {
                foreach (var linkedTowerID in Towers[i].Data.LinkedTowerIDs)
                {
                    Gizmos.DrawLine(Towers[i].transform.position, GetTower(linkedTowerID).transform.position);
                }
            }
        }
    }
    
}