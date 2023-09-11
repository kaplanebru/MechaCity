using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;
using System.Linq;
using Unity.Netcode;

public class TeamsHandler : MonoBehaviour
{
    public Team[] teams;

    private void OnEnable()
    {
        teams = GetComponentsInChildren<Team>();
        Eventbus.TeamEvents.OnTeamChange += ExchangeTower;
        Eventbus.FireEvents.OnTowerKilled += SendGridByTeam;

        Eventbus.NetworkEvents.OnPlayerSpawned += SetPlayerForTeam;
    }

   

    private void SetPlayerForTeam(Player player, ulong id)
    {
        teams[id].Data.Player = player;
        player.Setup();
    }

    Team GetTeamDataByTeamType(TeamType type) => teams.First(team => team.Data.TeamType == type);
    
    private void SendGridByTeam(Tower deadTower)
    {
         var team = GetTeamDataByTeamType(deadTower.Data.TeamTowerData.TeamType);
         Eventbus.FireEvents.OnTowerTeamDetection?.Invoke(new TowerGridRelationModel(team.Data.Grid, deadTower));
    }

    private void ExchangeTower(Tower deadTower)
    {
        //var perpetratorTeam = teams.FirstOrDefault(t => t.Data.TeamType == type)?.Data; //todo: team classına da yazılabilir griddeki gibi
        
        for (int i = 0; i < teams.Length; i++)
        {
            if (teams[i].Data.TeamType == deadTower.Data.TeamTowerData.TeamType)
            {
                teams[i].RemoveTower(deadTower);

                var otherTeam = teams[teams.Length - 1 - i];
                otherTeam.TakeTowerFromRival(deadTower);
                deadTower.SetTeam(otherTeam.Data.TeamTowerData);
                break;
            }
        }
    }
    
    private void OnDisable()
    {
        Eventbus.TeamEvents.OnTeamChange -= ExchangeTower;
        Eventbus.FireEvents.OnTowerKilled -= SendGridByTeam;
        
        Eventbus.NetworkEvents.OnPlayerSpawned -= SetPlayerForTeam;
    }
}