using System.Collections;
using System.Collections.Generic;
using Actor;
using Enums;
using Grid;
using UnityEngine;



public class GridToIndicator
{
    private uint[] _actors;
    private List<IndicatorGridData> indicatorDatas = new();
    
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
            indicatorDatas.Add(indicatorData);
        }
        
        IndicatorEvents.OnIndicatorGridDatasSet?.Invoke(indicatorDatas);
    }
}