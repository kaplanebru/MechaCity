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
        GeneralEventbus.InitializerEvents.OnActorsRegisteredToGrid += SetTowers;


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
    }

    void RegisterToTheGrid()
    {
        ActorDB.OrderRegistryByRow();
        ActorDB.OnRegistryUpdate();
    }
    
    private void SetTowers()
    {
        foreach (var newActor in ActorStarterDatas)
        {
            newActor.SetTowersNumericData();
            newActor.SetTowersVisualData(); //todo: test
        }
        
        FillAllTowers();
        SetTowerRelatedIDs();
        GeneralEventbus.InitializerEvents.OnActorsAndTowersInitiated?.Invoke();
    }

    public void FillAllTowers()
    {
        List<TowerObject> towers = new();
        foreach (var actorStarter in ActorStarterDatas)
        {
            towers.AddRange(actorStarter.TowerObjects);
        }
        AllTowers.ReceiveTowers(towers);
    }
    
    void SetTowerRelatedIDs()
    {
        foreach (var tower in AllTowers.Towers)
        {
            tower.initializer.SetTowerRelatedIds();
        }
        GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet?.Invoke();
    }
    private void OnDisable()
    {
        ActorDB.Unsubscribe();
        AllTowers.Unsubscribe();
        GeneralEventbus.InitializerEvents.OnActorsRegisteredToGrid -= SetTowers;
    }

 
}