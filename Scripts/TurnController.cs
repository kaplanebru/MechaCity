using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public List<Tower> towerGroup = new ();
    public Enums.TurnState state;
    public int maxTowersInGroup;

    private void OnEnable()
    {
        Eventbus.TowerEvents.OnTowerSelected += TowerSelected;
    }
    private void TowerSelected(Tower newTower)
    {
        //if not shown, show chain
        if(towerGroup.Count == maxTowersInGroup)
            towerGroup.Clear();
        
        towerGroup.Add(newTower);
        //chain position will be on towerGroup[0]
        //if more towers in the group, stretch chain
    }
    
    public void SelectionEnded()
    {
        state = Enums.TurnState.BoundState;
        //Enable Bound Towers Script
        Eventbus.TurnEvents.OnSelectionEnded?.Invoke(towerGroup);
        towerGroup.Clear();
        //Disable or Disappear GroupTowersButton
    }
    
    private void CheckState()
    {
        switch(state)
        {
            case Enums.TurnState.Selection:
                break;
            case Enums.TurnState.BoundState:
                break;
            case Enums.TurnState.TurnEnded:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
  
    private void OnDisable()
    {
        Eventbus.TowerEvents.OnTowerSelected -= TowerSelected;
    }

}
