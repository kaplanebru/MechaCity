using System.Collections;
using System.Collections.Generic;
using Actor;
using Enums;
using Grid;
using UnityEngine;

public class SlotInGameData
{
    public Slot Slot;
    public Dictionary<int, IndicatorState> TargetSlotStates = new();
    public Vector3 Position;
}

public class GridToIndicator
{
    private Dictionary<int, ActorData> actorsBySlots = new();
    Slot[] slots;
    private List<SlotInGameData> slotInGameDatas = new();
    private uint[] _actors;

    private List<IndicatorData> indicatorDatas = new();


    private Dictionary<ActorData, List<ActorData>> actorEdges = new();

    void SetIndicatorDatas()
    {
        indicatorDatas.Clear();
        
        foreach (var actorID in _actors)
        {
            var actor = ActorHolder.Registry[actorID];
            var indicatorData = new IndicatorData();
            
            indicatorData.StartPos = actor.Center; //actor.Towers[0]

            foreach (var targetID in actor.TargetActors)
            {
                var targetActor = ActorHolder.Registry[targetID];
                var targetPos = targetActor.Center;
                indicatorData.TargetPositions.Add(targetPos);

                indicatorData.TargetPosStates.Add(targetPos,
                    actor.Towers[0].TeamType == targetActor.Towers[0].TeamType
                        ? IndicatorState.Friendly 
                        : IndicatorState.Enemy);
            }
            indicatorDatas.Add(indicatorData);
        }
        
        IndicatorEvents.OnIndicatorDatasSet?.Invoke(indicatorDatas);
    }

    void SetSlotInGameDatas()
    {
        foreach (var slot in slots)
        {
            SlotInGameData slotInGame = new SlotInGameData();

            slotInGame.Slot = slot;
            var slotActor = actorsBySlots[slot.Id];
            slotInGame.Position = slotActor.Center;

            foreach (var targetSlot in slot.TargetSlots)
            {
                slotInGame.TargetSlotStates.Add(targetSlot,
                    actorsBySlots[targetSlot].Towers[0].TeamType == slotActor.Towers[0].TeamType
                        ? IndicatorState.Friendly
                        : IndicatorState.Enemy);
            } //double elemanlarından birbirine ok çıksın istemeyiz, o yüzden slot değil actor üzerinden

            slotInGameDatas.Add(slotInGame);
        }

        foreach (var actor in _actors)
        {
        }
    }


    void SendEdges()
    {
        foreach (var slot in slots)
        {
            var startActor = actorsBySlots[slot.Id];
            foreach (var targetSlot in slot.TargetSlots)
            {
                if (actorsBySlots[targetSlot].Towers[0].TeamType == startActor.Towers[0].TeamType)
                {
                }
            }
        }
    }
}