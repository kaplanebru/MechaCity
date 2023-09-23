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
    public TeamsHolder assetHolder;
    public List<Tower> allTowers = new();

    private void OnEnable()
    {
        Eventbus.TeamEvents.OnTeamChange += ExchangeTower;
        Eventbus.FireEvents.OnTowerKilled += SendGridByTeam;
        Eventbus.NetworkRequestEvents.OnPlayerSpawned += SetPlayerForTeam;
        
        CreateTeams();
        SetAllTowers();
    }
    
    

    void CreateTeams()
    {
        teams = new Team[assetHolder.Teams.Length];
        for (int i = 0; i < teams.Length; i++)
        {
            teams[i] = Instantiate(assetHolder.Teams[i], transform);
            teams[i].Initialize();
        }
        
        SetFirstMatches();
    }
    
    void SetFirstMatches() //Temporary
    {
        teams[0].LinkFirstMatches(teams[1]);
        teams[1].LinkFirstMatches(teams[0]);
    }

    void SetAllTowers()
    {
        foreach (var team in teams)
        {
            allTowers.AddRange(team.Data.Towers);
        }

        for (int i = 0; i < allTowers.Count; i++)
        {
            allTowers[i].Data.Id = i;
            allTowers[i].towerParts.SetClickableIds(i); //, Data.TeamTowerData.TeamType); //for MP
        }
    }

    private void SetPlayerForTeam(Player player, ulong id)
    {
        teams[id].Data.Player = player;
        player.Setup(teams[id].Data.TeamTowerData.TeamType, allTowers);

        foreach (var team in teams)
        {
            if (team.Data.Player == null)
            {
                print("Waiting for other players to join..."); //sadece client1'de görünmeli
                return;
            }
        }

        Eventbus.NetworkEvents.OnAllClientsSet?.Invoke(teams);
        print("Game Started");
    }

    Team GetTeamDataByTeamType(TeamType type) => teams.First(team => team.Data.TeamType == type);

    private void SendGridByTeam(Tower deadTower)
    {
        var team = GetTeamDataByTeamType(deadTower.Data.TeamTowerData.TeamType);
        Eventbus.FireEvents.OnTowerTeamDetection?.Invoke(new TowerGridRelationModel(team.Data.Grid, deadTower));
    }

    private void ExchangeTower(Tower deadTower)
    {
        Team oldTeam = GetTeamDataByTeamType(deadTower.Data.TeamTowerData.TeamType);
        Team newTeam = teams.FirstOrDefault(t => t != oldTeam);
        
        oldTeam.RemoveTower(deadTower);
        newTeam.TakeTowerFromRival(deadTower);
        print("old team: " + oldTeam + " newTeam: " + newTeam);
        //deadTower.SetTeam(newTeam.Data.TeamTowerData);

        // for (int i = 0; i < teams.Length; i++)
        // {
        //     if (teams[i].Data.TeamType == deadTower.Data.TeamTowerData.TeamType)
        //     {
        //         teams[i].RemoveTower(deadTower);
        //
        //         var otherTeam = teams[teams.Length - 1 - i];
        //         otherTeam.TakeTowerFromRival(deadTower);
        //         deadTower.SetTeam(otherTeam.Data.TeamTowerData);
        //         break;
        //     }
        // }
    }

    private void OnDisable()
    {
        Eventbus.TeamEvents.OnTeamChange -= ExchangeTower;
        Eventbus.FireEvents.OnTowerKilled -= SendGridByTeam;
        Eventbus.NetworkRequestEvents.OnPlayerSpawned -= SetPlayerForTeam;
    }
}