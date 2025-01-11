using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerRelated
{
    public class FloorCollection: TowerRelatedElementCollection<Floor>,IEnumeratorContainer
    {
        private List<Floor> selectedFloors = new();
        public override void Subscribe()
        {
            Eventbus.LinkEvents.OnLinkLoading += OperateFloors;
            Eventbus.LinkEvents.OnUnlink += ResetFloors;

            GeneralEventbus.InitializerEvents.OnMediatorElementsReady += HideAll;
        }

        public override void Initialize()
        {
            
        }
        
        private void OperateFloors(List<int> ids)
        {
            foreach (var id in ids)
            {
                var floor = Collection[id]; //Group.FirstOrDefault(f => f.Id == id);
                selectedFloors.Add(floor);
                floor.ShowGear();
            }

            //StartCoroutine(nameof(LeCoroutine));
            OpenFloors(ids);
        }

        private async void OpenFloors(List<int> ids)
        {
            await DelayMaker.WaitForSeconds(.5f);
            FloorsOpenedCall(ids);
        }
        void FloorsOpenedCall(List<int> ids)
        {
            MediatorEventbus.ChainLinkEvents.OnFloorsOpened?.Invoke(ids.ToArray());
        }

        void HideAll()
        {
            foreach (var floor in Collection.Values)
            {
                floor.TurnOffGear();
            }
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
            Eventbus.LinkEvents.OnLinkLoading -= OperateFloors;
            Eventbus.LinkEvents.OnUnlink -= ResetFloors;
            
            GeneralEventbus.InitializerEvents.OnMediatorElementsReady -= HideAll;
        }


        public IEnumerator LeCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            //FloorsOpenedCall();
            yield break;
        }
    }
}
