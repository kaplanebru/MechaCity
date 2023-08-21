using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;



public abstract class BasePlayer: MonoBehaviour //<TPlayerData>: MonoBehaviour where TPlayerData : PlayerData
{
    public PlayerData Data;
    public void Initialize()
    {
        var towersPb = Instantiate(Data.TowersPrefab);
        Data.Towers = towersPb.GetComponentsInChildren<Tower>().ToList();
        SetGrid();
        EnumerateAllTowers();
    }
    
    public void LinkFirstMatches(BasePlayer rivalPlayer) //Temporary
    {
        for (int i = 0; i < Data.Towers.Count; i++)
        {
            Data.Towers[i].Data.LinkedTowers.Add(rivalPlayer.Data.Towers[i]);
        }
    }

    void EnumerateAllTowers()
    {
        for (int i = 0; i < Data.Towers.Count; i++)
        {
            Data.Towers[i].Data.Id = i;
        }
    }

    void SetGrid()
    {
        Data.Grid.Initialize(this);
    }
}
