using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Datas;
using UnityEngine;


[Serializable]
public class TeamConstructorData
{
    public Transform TowersPrefab;
}

public class Team: MonoBehaviour //<TPlayerData>: MonoBehaviour where TPlayerData : TeamData
{
    public TeamData Data;
    [SerializeField] TeamConstructorData ConstructorData;
    public void Initialize()
    {
        AssignTowers();
        SetGrid();
        SetTowers();
    }

    void AssignTowers()
    {
        var towersPb = Instantiate(ConstructorData.TowersPrefab, transform);
        Data.Towers = towersPb.GetComponentsInChildren<Tower>().ToList();
    }
    
    void SetGrid()
    {
        Data.Grid.Initialize(this);
    }
    void SetTowers()
    {
        for (int i = 0; i < Data.Towers.Count; i++)
        {
            Data.Towers[i].Data.SlotId = i;
            Data.Towers[i].Setup(Data.TeamTowerData);
        }
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


    
}
