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
        Shield,
        DisarmSign,
        MultiShooter,
        Shooter,
        Health,
        Lock,
        Bridge,
    }
    public class DBTowerRelatedCollections : MonoBehaviour
    {
        private Dictionary<TowerRelatedType, ITowerRelatedCollection> registry = new ();
        private GearCollection gearCollection;
        
        private void OnEnable()
        {
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady += Initialize;
        }
        
        private void Initialize()
        {
            FillCollectionsRegistry();
            SubscribeToCollections();
            GeneralEventbus.InitializerEvents.OnExternalElementsReady?.Invoke();
        }

        
        
        void FillCollectionsRegistry()
        {
            registry.Add(TowerRelatedType.Floor, new FloorCollection(GetComponentsInChildren<Floor>()));
            registry.Add(TowerRelatedType.Shield, new ShieldCollection(GetComponentsInChildren<Shield>()));
            registry.Add(TowerRelatedType.MultiShooter, new MultiShooterCollection(GetComponentsInChildren<MultiShooter>()));
            registry.Add(TowerRelatedType.DisarmSign, new DisarmSignCollection(GetComponentsInChildren<DisarmSign>()));
            
            gearCollection = new GearCollection(GetComponentsInChildren<IGear>());
        }


        void SubscribeToCollections()
        {
            foreach (var group in registry.Values)
            {
                group.Subscribe();
            }
            gearCollection.Subscribe();
        }

        void UnsubscribeFromCollections()
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
            UnsubscribeFromCollections();
        }
    }
}