using System;
using Actor;
using Towers;
using UnityEngine;

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
            
            towerObject.initializer = new TowerInitializer(towerObject);
            towerObject.initializer .DataSetup();
            towerObject.initializer.DataVisualCorrespondenceSetup(ActorData.TeamVisualData);
            
            ActorData.TowerIDs[i] = towerObject.Data.UniqID;
            ActorData.Towers[i] = towerObject.Data;
            ActorData.Towers[i].SetClickHandlerID(ActorData.ID); //todo: bu her set teamde tekrarlanmalı
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