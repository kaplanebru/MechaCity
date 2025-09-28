using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Actor;
using Enums;
using Towers;
using UnityEngine;

public class ActorsInitializer : MonoBehaviour
{
    public ActorStarterData[] ActorStarterDatas;
    public CommonData commonData;
    private AllTowers AllTowers;
    private ActorDB ActorDB = new();

    private void OnEnable()
    {
        ActorDB.Initialize();
        AllTowers = new AllTowers(commonData.MaxTowerHeight);
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
        ActorDB.OnRegistryStart();
    }

    private void ProcessTowersByActorData()
    {
        SetTowers();
        FillAllTowers();
        GeneralEventbus.InitializerEvents.OnActorsAndTowersReady?.Invoke();
        
        ExecuteVisuals(); //1-2 sn geciktirilebilir
        GeneralEventbus.InitializerEvents.OnActorsRegisteredToGrid -= ProcessTowersByActorData;
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
            if(newActor.ActorData.Type == ActorType.MultiTower)
                newActor.OnDoubleCase();
        }
    }
    
    private void OnDisable()
    {
        ActorDB.Unsubscribe();
        AllTowers.Unsubscribe();
        //GeneralEventbus.InitializerEvents.OnActorsRegisteredToGrid -= ProcessTowersByActorData;
    }
}