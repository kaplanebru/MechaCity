using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Towers
{
    public class TowerRelatedsInitializer
    {
        public void Subscribe()
        {
            GeneralEventbus.InitializerEvents.OnActorsAndTowersReady += SetTowerRelateds;
            GeneralEventbus.InitializerEvents.OnTeamsAndClientsSet += ExecuteVisualElements;
        }

        void SetTowerRelateds()
        {
            SetTowerRelatedIDs();
            SetTowerBpElementsData();//todo: biraz geciktirilebilir
        }
        void SetTowerRelatedIDs()
        {
            foreach (var tower in AllTowers.Towers)
            {
                tower.initializer.SetTowerRelatedIds();
            }
            GeneralEventbus.InitializerEvents.OnTowerRelatedIDsSet?.Invoke();
        }
        
        void SetTowerBpElementsData()
        {
            foreach (var tower in AllTowers.Towers)
            {
                tower.initializer.TowerBPElementsDataSetup();
            }
        }
        
        private void ExecuteVisualElements()
        {
            foreach (var tower in AllTowers.Towers)
            {
                var data = tower.Data.NumericData;
                if(data.LockStatus.Locked)
                    Eventbus.TowerEvents.OnLock?.Invoke(data.LockStatus.Limit, data.UniqID);

                tower.initializer.ExecuteVisualsAfterSetup();
            }
        }
        public void Unsubscribe()
        {
            GeneralEventbus.InitializerEvents.OnActorsAndTowersReady -= SetTowerRelateds;
            GeneralEventbus.InitializerEvents.OnTeamsAndClientsSet -= ExecuteVisualElements;
        }
    }

}
