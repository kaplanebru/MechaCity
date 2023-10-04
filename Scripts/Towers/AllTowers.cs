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
        public static List<Tower> Towers = new();

        [SerializeField] List<Tower> _towers = new();
        public Transform[] TowersPrefab;
        [SerializeField] TowersDataHolder towerDatas;


        public static Tower GetTower(int id) => Towers[id]; //Towers.FirstOrDefault(t => t.Data.UniqID == id);

        private void OnEnable()
        {
            CreateTowers();
        }

        void CreateTowers()
        {
            InstantiateTowers();
            AssignTowers();
            TowerEvents.OnTowersCreated?.Invoke();
        }

        void InstantiateTowers()
        {
            foreach (var prefab in TowersPrefab)
            {
                var towersPb = Instantiate(prefab, transform);
                _towers.AddRange(towersPb.GetComponentsInChildren<Tower>().ToList());
            }

            Towers = _towers;
        }

        void AssignTowers()
        {
            for (int i = 0; i < Towers.Count; i++)
            {
                var tower = Towers[i];
                tower.Data = towerDatas.GetTowerData(i);
                tower.Data.UniqID = i;
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