using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerRelated
{
    public class MediatorCollectionHandler : MonoBehaviour
    {
        private GearCollection gearCollection;
        
        private void OnEnable()
        {
            GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet += Initialize;
        }
        
        private void Initialize()
        {
            FillCollectionsRegistry();
            SubscribeToCollections();
            GeneralEventbus.InitializerEvents.OnMediatorElementsReady?.Invoke();
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
            GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet -= Initialize;
            UnsubscribeFromCollections();
        }
    }
}