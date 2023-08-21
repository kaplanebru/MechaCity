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
        SetAllTowers();
    }
    
    public void LinkFirstMatches(BasePlayer rivalPlayer) //Temporary
    {
        for (int i = 0; i < Data.Towers.Count; i++)
        {
            Data.Towers[i].LinkedTowers.Add(rivalPlayer.Data.Towers[i]);
        }
    }

    void SetAllTowers()
    {
        for (int i = 0; i < Data.Towers.Count; i++)
        {
            Data.Towers[i].Id = i;
            Data.Towers[i].Setup(this);
        }
    }

    void SetGrid()
    {
        Data.Grid.Initialize(this);
    }
}
