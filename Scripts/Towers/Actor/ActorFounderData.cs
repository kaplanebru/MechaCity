using System;
using System.Linq;
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

        ActorData.TargetActors = new();
        ActorData.Neighbours = new();
        ActorData.ActivityStatus = new();
    }

    public void StartActorsAndSetTowers()
    {
        SetID();
        AllocateCollections();
        
        InitializeTowersByActor();
        ActorData.RegisterTowersAutonomously(TowerObjects.Select(t=>t.Data).ToArray());
        ActorData.SetCenterAutonomously(TowerObjects);
        
        ActorData.Health = ActorData.InitialHealth;
        AddActorToRegistry();
    }

    void InitializeTowersByActor()
    {
        foreach (var towerObject in TowerObjects)
        {
            towerObject.initializer = new TowerInitializer(towerObject);
            towerObject.initializer.DataSetup();
            towerObject.initializer.DataVisualCorrespondenceSetup(ActorData.TeamVisualData);
            towerObject.Data.SetClickHandlerID(ActorData.ID);
        }
    }

    // private void SetCenterAutonomously()
    // {
    //     var center = Vector3.zero;
    //     foreach (var tower in  TowerObjects)
    //     {
    //         center += tower.transform.position;
    //     }
    //     
    //     center /= ActorData.TowerAmount;
    //     ActorData.Center = center;
    // }
    
    private void AddActorToRegistry()
    {
        ActorHolder.Registry.Add(ActorData.ID, ActorData);
    }
}