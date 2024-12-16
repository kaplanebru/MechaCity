using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class FloorCollection: BaseTowerRelatedCollection<Floor> ,IEnumeratorContainer
    {
      
        private List<Floor> selectedFloors = new();
        public FloorCollection(Floor[] collection) : base(collection)
        {
        }

        public override void Subscribe()
        {
            Eventbus.LinkEvents.OnLinkLoading += OpenFloors;
            Eventbus.LinkEvents.OnUnlink += ResetFloors;

            GeneralEventbus.InitializerEvents.OnExternalElementsReady += HideAll;
        }
        
    
        private void OpenFloors(List<int> ids)
        {
            foreach (var id in ids)
            {
                var floor = Collection[id]; //Group.FirstOrDefault(f => f.Id == id);
                selectedFloors.Add(floor);
                floor.ShowGear();
            }
    
            GeneralEventbus.OnCoroutineTrigger?.Invoke(this); //todo: temp
        }

        void HideAll()
        {
            foreach (var floor in Collection.Values)
            {
                floor.TurnOffGear();
            }
        }
    
        void FloorsOpenedCall()
        {
            Eventbus.LinkEvents.OnFloorsOpened?.Invoke();
        }
        
        private void ResetFloors(List<int> ids)
        {
            foreach (var floor in selectedFloors)
            {
                floor.HideGear();
            }
            selectedFloors.Clear();
        }
        
        public override void Unsubscribe()
        {
            Eventbus.LinkEvents.OnLinkLoading -= OpenFloors;
            Eventbus.LinkEvents.OnUnlink -= ResetFloors;
            
            GeneralEventbus.InitializerEvents.OnExternalElementsReady -= HideAll;
        }


        public IEnumerator LeCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            FloorsOpenedCall();
        }
    }
}
