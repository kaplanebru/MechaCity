using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public interface ITowerRelatedCollection
    {
        public void Subscribe();
        public void Unsubscribe();
    }

    public enum TowerRelatedType
    {
        Floor,
        Gear,
        Shooter,
        Health,
        Lock,
        Shield,
        Bridge,
        DisarmSign,
        MultiShooter
    }
    public class TowerExternalElementsDatabase : MonoBehaviour
    {
        public TowerRelatedElementDataBase dataBase;
        private Dictionary<TowerRelatedType, ITowerRelatedCollection> registry = new ();
        private GearCollection gearCollection;
     

        private void OnEnable()
        {
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady += Initialize;
        }
        
        private void Initialize()
        {
            FillRelatedElementData();
            FillRegistry();
            SubscribeToGroups();
            GeneralEventbus.InitializerEvents.OnExternalElementsReady?.Invoke();
        }

        void FillRelatedElementData()
        {
            dataBase.Floors = GetComponentsInChildren<Floor>();
            dataBase.IGears = GetComponentsInChildren<IGear>();
            dataBase.Shields = GetComponentsInChildren<Shield>();
            dataBase.MultiShooters = GetComponentsInChildren<MultiShooter>();
            dataBase.DisarmSigns = GetComponentsInChildren<DisarmSign>();
        }
        
        void FillRegistry()
        {
            registry.Add(TowerRelatedType.Floor, new FloorCollection(dataBase.Floors));
            registry.Add(TowerRelatedType.Shield, new ShieldCollection(dataBase.Shields));
            registry.Add(TowerRelatedType.MultiShooter, new MultiShooterCollection(dataBase.MultiShooters));
            registry.Add(TowerRelatedType.DisarmSign, new DisarmSignCollection(dataBase.DisarmSigns));
            
            gearCollection = new GearCollection(dataBase.IGears.ToArray());
        }


        void SubscribeToGroups()
        {
            foreach (var group in registry.Values)
            {
                group.Subscribe();
            }
            gearCollection.Subscribe();
        }

        void UnsubscribeFromGroups()
        {
            foreach (var group in registry.Values)
            {
                group.Unsubscribe();
            }
            gearCollection.Unsubscribe();
        }

        private void OnDisable()
        {
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady -= Initialize;
            UnsubscribeFromGroups();
        }
    }
}