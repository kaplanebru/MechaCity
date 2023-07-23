using System;
using System.Collections;
using System.Collections.Generic;
using Datas;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public int moveCounter;
    public Enums.TurnState state;

    private void OnEnable()
    {
        Eventbus.TowerEvents.OnTowerSelected += TowerSelected;
        Eventbus.TurnEvents.OnTurnStarted += FirstMove;
        Eventbus.TurnEvents.OnTurnEnded += TurnEnded;
        
    }

    private void TowerSelected()
    {
        CheckState();
    }

    private void CheckState()
    {
        switch(state)
        {
            case Enums.TurnState.TurnStarted:
                FirstMove();
                break;
            case Enums.TurnState.TurnPlaying:
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
        Eventbus.TurnEvents.OnTurnStarted -= FirstMove;
        Eventbus.TurnEvents.OnTurnEnded -= TurnEnded;
    }


    private void FirstMove()
    {
        state = Enums.TurnState.TurnStarted;
        //ShowChain();
        //MoveChainRoutine()
        //test
    }

    void NextMove()
    {
        moveCounter++;
    }
    
    private void TurnEnded()
    {
        state = Enums.TurnState.TurnEnded;
        moveCounter = 0;
    }
}
