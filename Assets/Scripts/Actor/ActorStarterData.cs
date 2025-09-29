using System;
using System.Linq;
using Actor;
using Towers;
using UnityEngine;

[Serializable]
public class ActorStarterData
{
    public ActorData ActorData;
    public TowerObject[] TowerObjects;

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

    public void StartActorForGrid()
    {
        SetID();
        AllocateCollections();
        InitiateTowers();
        
        ActorData.SetCenterAutonomously(TowerObjects);
        AddActorToDBRegistry();
    }
    private void AddActorToDBRegistry()
    {
        ActorDB.Registry.Add(ActorData.ID, ActorData);
    }

    void InitiateTowers()
    {
        ActorData.TowerIDs = TowerObjects.Select(t => t.Data.NumericData.UniqID).ToArray();
        ActorData.TowerNumericDatas = TowerObjects.Select(t => t.Data.NumericData).ToArray();
        ActorData.Towers = TowerObjects.Select(t => t.Data).ToArray();//bu sonra eklenebilir: esasen visual datayla ilgili
    }

    public void SetTowersNumericData()
    {
        foreach (var towerObject in TowerObjects)
        {
            towerObject.initializer = new TowerInitializer(towerObject);
            towerObject.initializer.NumericDataInitialSetup(ActorData.TeamType);
        }
        ActorData.OrderTowerDataByHeight();
    }

    public void SetTowersVisualData()
    {
        foreach (var towerObject in TowerObjects)
        {
            towerObject.initializer.VisualDataIdentification(); //bu 3ü aslında execution ile ilgili, yani daha sonra gelebilir.
            towerObject.initializer.VisualDataInitialSetup(ActorData.TeamVisualData);
            towerObject.Data.VisualData.SetClickHandlerID(ActorData.ID);
        }
       
    }
  
    public void OnDoubleCase()
    {
        var newDouble = new DoubleTowerPhysical(ActorData.TowerNumericDatas, ActorData.Towers);
        newDouble.Equalize();
        newDouble.CreateBridge();
    }
    
   
   
}