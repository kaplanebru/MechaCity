using System.Collections;
using System.Collections.Generic;
using Actor;
using Towers;
using UnityEngine;

public class ActorsInitializer : MonoBehaviour
{
    public ActorFounderData[] actorFounderDatas;
    private AllTowers AllTowers = new();
    private ActorHolder ActorHolder = new();

    private void OnEnable()
    {
        ActorHolder.Initialize();
        AllTowers.Subscribe();

        Invoke(nameof(FoundActorsWithTowers), .5f); //diğer on enable getcomponentlar çalışsın diye
    }

    public void FoundActorsWithTowers()
    {
        foreach (var actorFounder in actorFounderDatas)
        {
            actorFounder.StartActorsAndSetTowers();
        }
        FillAllTowers();
        InitiateGrid();
        
        GeneralEventbus.InitializerEvents.OnActorsCreated?.Invoke();
    }

    void InitiateGrid()
    {
        ActorHolder.OrderRegistry();
        ActorHolder.OnRegistryUpdate();
    }

    public void FillAllTowers()
    {
        List<Tower> towers = new();
        foreach (var actorFounder in actorFounderDatas)
        {
            towers.AddRange(actorFounder.TowerObjects);
        }
        AllTowers.ReceiveTowers(towers);
    }
    private void OnDisable()
    {
        ActorHolder.Unsubscribe();
        AllTowers.Unsubscribe();
    }
}