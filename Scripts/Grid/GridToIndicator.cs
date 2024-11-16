using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using Grid;
using UnityEngine;


public class GridToIndicator
{
    private uint[] _actors;
    private Dictionary<uint, IndicatorGridData> indicatorDatas = new();

    public void Subscribe()
    {
        Eventbus.CombatEvents.OnActorKilled += UpdateIndicatorState;
    }
    public void SetIndicatorDatas(uint[] actors)
    {
        _actors = actors;
        indicatorDatas.Clear();

        foreach (var actorID in _actors)
        {
            var actor = ActorHolder.Registry[actorID];
            var indicatorData = new IndicatorGridData();

            indicatorData.ActorID = actorID;
            indicatorData.StartPos = actor.Center; //actor.Towers[0]

            SetTargets(actor, indicatorData);
            indicatorDatas.Add(actorID, indicatorData);
        }
        SendIndicatorDatas();
    }

    void SetTargets(ActorData actor, IndicatorGridData indicatorData)
    {
        indicatorData.TargetPositions.Clear(); //for update
        indicatorData.TargetStates.Clear(); //for update

       
        foreach (var targetID in actor.TargetActors)
        {
            var targetActor = ActorHolder.Registry[targetID];

            var targetPos = targetActor.Center;
            indicatorData.TargetPositions.Add(targetPos);

           
            indicatorData.TargetStates.Add(targetPos,
                actor.Towers[0].TeamType == targetActor.Towers[0].TeamType
                    ? IndicatorState.Friendly
                    : IndicatorState.Enemy);
        }
    }

    private void UpdateIndicatorState(uint actorID) //todo: on tower died
    {
        UpdateDeadIndicator(actorID);
        UpdateRoverIndicators(actorID);
        SendIndicatorDatas();
    }

    void UpdateDeadIndicator(uint actorID)
    {
        var deadActor = ActorHolder.Registry[actorID];
        var deadIndicator = indicatorDatas[actorID];
        SetTargets(deadActor, deadIndicator);
    }

    void UpdateRoverIndicators(uint actorID)
    {
        foreach (var roverID in _actors)
        {
            var rover = ActorHolder.Registry[roverID];
            if (rover.TargetActors.Contains(actorID))
            {
                var roverIndicator = indicatorDatas[roverID];
                SetTargets(rover, roverIndicator);
            }
        }
    }

    void SendIndicatorDatas()
    {
        IndicatorEvents.OnIndicatorGridDatasSet?.Invoke(indicatorDatas.Values.ToArray());
    }

    public void Unsubscribe()
    {
        Eventbus.CombatEvents.OnActorKilled -= UpdateIndicatorState;
    }
}