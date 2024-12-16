using System;
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

        Invoke(nameof(FoundActorsWithTowers), .5f);
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

[Serializable]
public class ActorFounderData
{
    public ActorData ActorData;
    public Tower[] TowerObjects;

    void SetID()
    {
        ActorData.ID = UniqueIdGenerator.UIntId();
    }

    void AllocateCollections()
    {
        ActorData.TowerAmount = TowerObjects.Length;
        ActorData.TowerIDs = new int [ActorData.TowerAmount];
        ActorData.Towers = new TowerData[ActorData.TowerAmount];
    }

    public void StartActorsAndSetTowers()
    {
        SetID(); //todo: clickable idleri check et
        AllocateCollections();
        SetActorCenterByFounderTowers();
        SetActorTowers();
        ActorData.Health = ActorData.InitialHealth;
        AddActorToRegistry();
    }
    
    private void SetActorTowers()
    {
        for (var i = 0; i < TowerObjects.Length; i++)
        {
            var towerObject = TowerObjects[i];
            
            var towerInitializer = new TowerInitializer(towerObject);
            towerInitializer.DataSetup();
            towerInitializer.DataVisualCorrespondenceSetup(ActorData.TeamVisualData);
            
            ActorData.TowerIDs[i] = towerObject.Data.UniqID;
            ActorData.Towers[i] = towerObject.Data;
            ActorData.Towers[i].SetClickHandlerID(ActorData.ID);
            ActorData.OrderTowerDataByHeight();

            ActorData.TargetActors = new();
            ActorData.Neighbours = new();
            ActorData.ActivityStatus = new();
        }
    }
    
    private void SetActorCenterByFounderTowers()
    {
        var center = Vector3.zero;
        foreach (var tower in  TowerObjects)
        {
            center += tower.transform.position;
        }
        
        center /= ActorData.TowerAmount;
        ActorData.Center = center;
    }
    
    private void AddActorToRegistry()
    {
        ActorHolder.Registry.Add(ActorData.ID, ActorData);
    }
}