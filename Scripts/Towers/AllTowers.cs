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

        [SerializeField] Transform levelPrefab;
        Transform _level;
        


        public static Tower GetTower(int id) => Towers[id]; //Towers.FirstOrDefault(t => t.Data.UniqID == id);
        public static TowerData GetData(int id) => Datas[id]; //Towers[id].Data;//Datas[i];


        private void OnEnable()
        {
            CreateTowers();
        }

        void CreateTowers()
        {
            InstantiateLevelPrefab();
            ReceiveTowers();
            ReceiveTowerData();
            SetFirstMatches();

            TowerEvents.OnTowersCreated?.Invoke();
        }

        void InstantiateLevelPrefab()
        {
            _level = Instantiate(levelPrefab, transform);
        }

        void ReceiveTowers()
        {
            Towers.AddRange(_level.GetComponentsInChildren<Tower>().ToList());
            TowersCount = Towers.Count;
        }
        void ReceiveTowerData()
        {
            for (int i = 0; i < TowersCount; i++)
            {
                Datas.Add(Towers[i].Data);
            }
        }
        // private readonly CombatData Data = new();
        // void CreateCombatPairsById() //is even ve initialda sadece
        // {
        //     for (var i = 0; i < AllTowers.Datas.Count-1; i++)
        //     {
        //         Data.CombatPairs.Add(new CombatPair(AllTowers.Datas[i], AllTowers.Datas[i+1], true));
        //     }
        //     Data.CombatPairs.Add(new CombatPair(AllTowers.Datas.Last(), AllTowers.Datas.First(), true));
        // }

        void SetFirstMatches()
        {
            for (int i = 0; i < Datas.Count-1; i++)
            {
                Datas[i].LinkedTowerIDs.Add(Datas[i+1].UniqID);
                Datas[i+1].LinkedTowerIDs.Add(Datas[i].UniqID);
            }
            
            Datas.Last().LinkedTowerIDs.Add(Datas.First().UniqID);
            
            // int teamTowerAmount = TowersCount / 2;
            // for (var i = 0; i < teamTowerAmount; i++)
            // {
            //     Datas[i].LinkedTowerIDs.Add(Datas[i + teamTowerAmount].UniqID);
            //     Datas[i + teamTowerAmount].LinkedTowerIDs.Add(Datas[i].UniqID);
            // }
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
                    if (Towers[i] == null) continue;
                    Gizmos.DrawLine(Towers[i].transform.position, GetTower(linkedTowerID).transform.position);
                }
            }
        }
    }
}