using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class DBTowerRelatedCollections : MonoBehaviour
    {
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
            gearCollection = new GearCollection(GetComponentsInChildren<IGear>());
        }


        void SubscribeToCollections()
        {
            gearCollection.Subscribe();
        }

        void UnsubscribeFromCollections()
        {
            gearCollection.Unsubscribe();
        }

        private void OnDisable()
        {
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady -= Initialize;
            UnsubscribeFromCollections();
        }
    }
}