using System;
using System.Linq;
using Actor;
using Towers;
using UnityEngine;

[Serializable]
public class ActorStarterData
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

    public void StartActorForGrid()
    {
        SetID();
        AllocateCollections();
        
        ActorData.TowerIDs = TowerObjects.Select(t => t.NumericData.UniqID).ToArray();
        ActorData.SetCenterAutonomously(TowerObjects);
        AddActorToDBRegistry();
    }
    private void AddActorToDBRegistry()
    {
        ActorDB.Registry.Add(ActorData.ID, ActorData);
    }

    public void InitiateActorTowers()
    {
        ActorData.TowerNumericDatas = TowerObjects.Select(t => t.NumericData).ToArray();
        foreach (var towerObject in TowerObjects)
        {
            towerObject.initializer = new TowerInitializer(towerObject);
            towerObject.initializer.NumericDataInitialSetup(ActorData.TeamType);
        }
        
        ActorData.SetTowerHeightCouple();
    }
    
    // void InitializeTowersVisualData()
    // {
    //     foreach (var towerObject in TowerObjects)
    //     {
    //         towerObject.initializer.VisualDataIdentification(); //bu 3ü aslında execution ile ilgili, yani daha sonra gelebilir.
    //         towerObject.initializer.VisualDataInitialSetup(ActorData.TeamVisualData);
    //         towerObject.Data.SetClickHandlerID(ActorData.ID);
    //     }
    // }
    //
    // void OnDoubleCase()
    // {
    //     var newDouble = new DoubleTowerPhysical(ActorData.Towers);
    //     //newDouble.Equalize();//todo later
    //     //todo: healthlerin de ortak yapılması lazım
    //     newDouble.CreateBridge(); //todo: bridgeler de hazır değil. execute afterda yapılması lazım bunların
    // }
    
   
}