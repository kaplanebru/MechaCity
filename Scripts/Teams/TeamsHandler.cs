using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;
using System.Linq;

public class TeamsHandler : MonoBehaviour
{
    public Team[] teams;

    private void OnEnable()
    {
        teams = GetComponentsInChildren<Team>();
        Eventbus.TeamEvents.OnTeamChange += ExchangeTower;
    }

    public void ExchangeTower(Tower deadTower)
    {
        //var perpetratorTeam = teams.FirstOrDefault(t => t.Data.TeamType == type)?.Data; //todo: team classına da yazılabilir griddeki gibi
        for (int i = 0; i < teams.Length; i++)
        {
            if (teams[i].Data.TeamType == deadTower.Data.teamCosmeticData.teamType)
            {
                teams[i].RemoveTower(deadTower);

                var otherTeam = teams[teams.Length - 1 - i];
                otherTeam.TakeTowerFromRival(deadTower);
                deadTower.SetTeam(otherTeam.Data.teamCosmeticData);
                break;
            }
        }
    }
    
    private void OnDisable()
    {
        Eventbus.TeamEvents.OnTeamChange -= ExchangeTower;
    }
}