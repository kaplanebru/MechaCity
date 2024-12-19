using System.Collections;
using System.Collections.Generic;
using Actor;
using Towers;
using UnityEngine;

public class ActorsInitializer : MonoBehaviour
{
    public ActorStarterData[] ActorStarterDatas;
    private AllTowers AllTowers = new();
    private ActorDB ActorDB = new();

    private void OnEnable()
    {
        ActorDB.Initialize();
        AllTowers.Subscribe();
        GeneralEventbus.InitializerEvents.OnActorsRegisteredToGrid += InitiateActorTowers;


        Invoke(nameof(InitiateActorsForGridRegistry), .5f); //diğer on enable getcomponentlar çalışsın diye
    }

    public void InitiateActorsForGridRegistry()
    {
        foreach (var newActor in ActorStarterDatas)
        {
            newActor.StartActorForGrid();
        }
        // FillAllTowers();
        RegisterToTheGrid();
        
        //GeneralEventbus.InitializerEvents.OnActorsInitiated?.Invoke();
    }

    void RegisterToTheGrid()
    {
        ActorDB.OrderRegistryByRow();
        ActorDB.OnRegistryUpdate();
    }
    
    private void InitiateActorTowers()
    {
        foreach (var newActor in ActorStarterDatas)
        {
            newActor.InitiateActorTowers();
        }
    }

    // public void FillAllTowers()
    // {
    //     List<Tower> towers = new();
    //     foreach (var actorFounder in ActorFounderDatas)
    //     {
    //         towers.AddRange(actorFounder.TowerObjects);
    //     }
    //     AllTowers.ReceiveTowers(towers);
    // }
    private void OnDisable()
    {
        ActorDB.Unsubscribe();
        AllTowers.Unsubscribe();
        GeneralEventbus.InitializerEvents.OnActorsRegisteredToGrid -= InitiateActorTowers;
    }

 
}