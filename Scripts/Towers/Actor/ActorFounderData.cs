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
        //OnDoubleCase();
        AddActorToRegistry();
    }

    void InitializeTowersByActor()
    {
        foreach (var towerObject in TowerObjects)
        {
            towerObject.initializer = new TowerInitializer(towerObject);
            towerObject.initializer.NumericDataSetup();
            towerObject.initializer.VisualDataIdentification(ActorData.TeamVisualData);
            towerObject.Data.SetClickHandlerID(ActorData.ID);
        }
    }

    void OnDoubleCase()
    {
        var newDouble = new DoubleTowerPhysical(ActorData.Towers);
        //newDouble.Equalize();//todo later
        //todo: healthlerin de ortak yapılması lazım
        newDouble.CreateBridge(); //todo: bridgeler de hazır değil. execute afterda yapılması lazım bunların
    }
    
    private void AddActorToRegistry()
    {
        ActorHolder.Registry.Add(ActorData.ID, ActorData);
    }
}