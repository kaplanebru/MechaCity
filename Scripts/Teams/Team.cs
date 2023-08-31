using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;



public class Team: MonoBehaviour //<TPlayerData>: MonoBehaviour where TPlayerData : TeamData
{
    public TeamData Data;
    public void Initialize()
    {
        var towersPb = Instantiate(Data.AssetHolder.TowersPrefab);
        Data.Towers = towersPb.GetComponentsInChildren<Tower>().ToList();
        SetGrid();
        SetAllTowers();
    }
    

    public void TakeTowerFromRival(Tower tower)
    {
        Data.Towers.Add(tower);
    }

    public void RemoveTower(Tower tower)
    {
        Data.Towers.Remove(tower);
    }

    public void LinkFirstMatches(Team rivalTeam) //Temporary
    {
        for (int i = 0; i < Data.Towers.Count; i++)
        {
            Data.Towers[i].Data.LinkedTowers.Add(rivalTeam.Data.Towers[i]);
        }
    }

    void SetAllTowers()
    {
        for (int i = 0; i < Data.Towers.Count; i++)
        {
            Data.Towers[i].Data.Id = i;
            Data.Towers[i].Setup(Data.TeamTowerData);
        }
    }

    void SetGrid()
    {
        Data.Grid.Initialize(this);
    }

    public void SetClickability(bool enable)
    {
        Data.Towers.ForEach(t=>t.Data.Clickable = enable);
    }

    private void OnDisable()
    {
        Data.Grid.DisableGrid();
    }
}
