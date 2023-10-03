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
        public List<Tower> allTowers;
        
        public Tower GetTowerByUniqID(int id) => allTowers.FirstOrDefault(t => t.Data.UniqID == id);

        private void OnEnable()
        {
            //Eventbus.TowerEvents.OnTowerRequestByID += GetTowerByUniqID;
            Eventbus.TeamEvents.OnTeamsSet += GetTowers;
        }

        private void GetTowers(Team[] teams)
        {
            foreach (var team in teams)
            {
                allTowers.AddRange(team.Data.Towers);
            }
        }

        private void OnDisable()
        {
            //Eventbus.TowerEvents.OnTowerRequestByID -= GetTowerByUniqID;
            Eventbus.TeamEvents.OnTeamsSet -= GetTowers;
        }
    }


}
