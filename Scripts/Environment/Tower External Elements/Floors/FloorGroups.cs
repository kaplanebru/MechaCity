using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class FloorGroups: IEnumeratorContainer
    {
        [SerializeField]private Floor[] _group;
        private List<Floor> selectedFloors = new();

        public FloorGroups(Floor[] group)
        {
            _group = group;
        }

        public void Subscribe()
        {
            Eventbus.LinkEvents.OnLinkLoading += OpenFloors;
            Eventbus.LinkEvents.OnUnlink += ResetFloors;

            GeneralEventbus.InitializerEvents.OnExternalElementsReady += OpenAll;
        }
        
    
        private void OpenFloors(List<int> ids)
        {
            foreach (var id in ids)
            {
                var floor = _group.FirstOrDefault(f => f.Id == id);
                selectedFloors.Add(floor);
                floor.Open();
            }
    
            GeneralEventbus.OnCoroutineTrigger?.Invoke(this); //todo: temp
        }

        void OpenAll()
        {
            foreach (var floor in _group)
            {
                floor.Open(true);
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
                floor.RestoreHeight();
            }
            selectedFloors.Clear();
        }
        
        public void Unsubscribe()
        {
            Eventbus.LinkEvents.OnLinkLoading -= OpenFloors;
            Eventbus.LinkEvents.OnUnlink -= ResetFloors;
            
            GeneralEventbus.InitializerEvents.OnExternalElementsReady -= OpenAll;
        }


        public IEnumerator LeCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            FloorsOpenedCall();
        }
    }
}
