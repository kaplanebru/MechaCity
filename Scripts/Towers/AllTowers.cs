using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Unity.VisualScripting;
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
        [SerializeField] List<Tower> towers = new();
        [SerializeField] TowerData[] datas;
        
        public Transform[] TowersPrefab;
        [SerializeField] TowersDataHolder constantDatas;
        


        public static Tower GetTower(int id) => Towers[id]; //Towers.FirstOrDefault(t => t.Data.UniqID == id);
        public static TowerData GetData(int id) => Towers[id].Data;//datas[i];
        

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
                towers.AddRange(towersPb.GetComponentsInChildren<Tower>().ToList());
            }

            Towers = towers;
            TowersCount = Towers.Count;
        }

        void CreateDatas()
        {
            datas = new TowerData [TowersCount];
            
            for (int i = 0; i < datas.Length; i++)
            {
                datas[i] = new TowerData(i);
            }
        }

        void AssignDatasToTowers()
        {
            for (int i = 0; i < TowersCount; i++)
            {
                var tower = Towers[i];
                tower.Data = datas[i];
                tower.ConstantData = constantDatas.Datas[i];
            }
            SetFirstMatches();
        }

        void SetFirstMatches()
        {
            int teamTowerAmount = TowersCount / 2;
            for (var i = 0; i < teamTowerAmount; i++)
            {
                datas[i].LinkedTowerIDs.Add(datas[i + teamTowerAmount].UniqID);
                datas[i + teamTowerAmount].LinkedTowerIDs.Add(datas[i].UniqID);
            }
           
        }
        
        public static void RestoreBullets()
        {
            Towers.ForEach(t => t.RestoreBullets());
        }


        private void OnDrawGizmos() //TODO: 2 kez çizilmiş oluyor, tek kez yapılması lazım. Hem Adan Bye hem Bden Aya olmamalı
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

    // private void OnDisable()
    // {
    //     //Eventbus.TeamEvents.OnTeamsSet -= GetTowers;
    //     //TowerEvents.OnInitialize -= GetTowers;
    // }

    // void GetTowers(Towers<Tower> towers)
    // {
    //     _towers.AddRange(towers);
    //     Towers = _towers;
    // }

    // void GetTowers(Team[] teams)
    // {
    //     foreach (var team in teams)
    //     {
    //        // _towers.AddRange(team.Data.TowerIds);
    //     }
    //
    //     Towers = _towers;
    // }
}