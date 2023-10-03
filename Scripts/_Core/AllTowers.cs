using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Teams;
using UnityEngine;


namespace Towers
{
    public class AllTowers : MonoBehaviour
    {
         private static List<Tower> _allTowers = new();

         readonly List<Tower> _towers = new();

        public static Tower GetTowerByID(int id) => _allTowers.FirstOrDefault(t => t.Data.UniqID == id);

        private void OnEnable()
        {
            Eventbus.TeamEvents.OnTeamsSet += GetTowers;
        }

        void GetTowers(Team[] teams)
        {
            foreach (var team in teams)
            {
                _towers.AddRange(team.Data.Towers);
            }

            _allTowers = _towers;
        }

        private void OnDisable()
        {
            Eventbus.TeamEvents.OnTeamsSet -= GetTowers;
        }
        
        private void OnDrawGizmos() //TODO: 2 kez çizilmiş oluyor, tek kez yapılması lazım. Hem Adan Bye hem Bden Aya olmamalı
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < _allTowers.Count/2; i++)
            {
                foreach (var linkedTowerID in _allTowers[i].Data.LinkedTowerIDs)
                {
                    Gizmos.DrawLine(_allTowers[i].transform.position, GetTowerByID(linkedTowerID).transform.position);
                }
            }
        }
    }


}
