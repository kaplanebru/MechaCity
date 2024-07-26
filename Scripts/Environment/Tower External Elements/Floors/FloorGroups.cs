using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class FloorGroups: IEnumeratorContainer
    {
        [SerializeField]private Floor[] _group;
        public float duration = 0.5f;
        public float openSize = 0.4f;
    
        private List<Floor> selectedFloors = new();

        public FloorGroups(Floor[] group)
        {
            _group = group;
        }

        public void Subscribe()
        {
            Eventbus.LinkEvents.OnLinkLoading += OpenFloors;
            Eventbus.LinkEvents.OnUnlink += ResetFloors;
        }
        
    
        private void OpenFloors(List<int> ids)
        {
            foreach (var id in ids)
            {
                var floor = _group.FirstOrDefault(f => f.Id == id);
                selectedFloors.Add(floor);
                floor.Open(openSize, duration);
            }
    
            GeneralEventbus.OnCoroutineTrigger?.Invoke(this); //todo: temp
        }
    
        void FloorsOpenedCall()
        {
            Eventbus.LinkEvents.OnFloorsOpened?.Invoke();
        }
        
        private void ResetFloors(List<int> ids)
        {
            foreach (var floor in selectedFloors)
            {
                floor.RestoreHeight(duration);
            }
            selectedFloors.Clear();
        }
        
        public void Unsubscribe()
        {
            Eventbus.LinkEvents.OnLinkLoading -= OpenFloors;
            Eventbus.LinkEvents.OnUnlink -= ResetFloors;
        }


        public IEnumerator LeCoroutine()
        {
            yield return new WaitForSeconds(duration);
            FloorsOpenedCall();
        }
    }
}
