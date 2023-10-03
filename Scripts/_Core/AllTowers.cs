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
            //Eventbus.TowerEvents.OnTowerRequestByID += GetTowerByID;
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
            //Eventbus.TowerEvents.OnTowerRequestByID -= GetTowerByID;
            Eventbus.TeamEvents.OnTeamsSet -= GetTowers;
        }
    }


}
