using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        GeneralEventbus.InitializerEvents.OnActorsRegisteredToGrid += ProcessTowersByActorData;
        
        Invoke(nameof(InitiateActorsForGridRegistry), .5f); //diğer on enable getcomponentlar çalışsın diye
    }

    public void InitiateActorsForGridRegistry()
    {
        foreach (var newActor in ActorStarterDatas)
        {
            newActor.StartActorForGrid();
        }
        RegisterToTheGrid();
    }

    void RegisterToTheGrid()
    {
        ActorDB.OrderRegistryByRow();
        ActorDB.OnRegistryUpdate();
    }

    private void ProcessTowersByActorData()
    {
        SetTowers();
        FillAllTowers();
        GeneralEventbus.InitializerEvents.OnActorsAndTowersReady?.Invoke();
        
        ExecuteVisuals(); //1-2 sn geciktirilebilir
    }
    
    private void SetTowers()
    {
        foreach (var newActor in ActorStarterDatas)
        {
            newActor.SetTowersNumericData();
            newActor.SetTowersVisualData(); //todo: test
        }
    }
    private void FillAllTowers()
    {
        List<TowerObject> towers = ActorStarterDatas.SelectMany(a => a.TowerObjects).ToList();
        AllTowers.ReceiveTowers(towers);
    }

    private void ExecuteVisuals()
    {
       SetActorDoubleCase();
       StartHealthVisualsForAll();
    }
    
    
    public void StartHealthVisualsForAll()
    {
        foreach (var actor in ActorDB.Registry.Keys)
        {
            ((HealthUnit) ActorDB.Units[Enums.ActorUnit.Health]).ResetHealth(actor);
        }
    }

    private void SetActorDoubleCase()
    {
        foreach (var newActor in ActorStarterDatas)
        {
            newActor.OnDoubleCase();
        }
    }
    
    private void OnDisable()
    {
        ActorDB.Unsubscribe();
        AllTowers.Unsubscribe();
        GeneralEventbus.InitializerEvents.OnActorsRegisteredToGrid -= ProcessTowersByActorData;
    }
}