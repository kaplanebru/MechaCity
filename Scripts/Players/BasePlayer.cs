using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;



public abstract class BasePlayer: MonoBehaviour //<TPlayerData>: MonoBehaviour where TPlayerData : PlayerData
{
    public PlayerData Data;
    public List<Tower> Towers = new();

    private void Start()
    {
        EnumerateAllTowers();
    }

    void EnumerateAllTowers()
    {
        for (int i = 0; i < Towers.Count; i++)
        {
            Towers[i].Data.Id = i;
        }
    }
}
