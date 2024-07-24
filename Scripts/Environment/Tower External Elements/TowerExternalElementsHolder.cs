using UnityEngine;

namespace TowerExternal
{
    public class TowerExternalElementsHolder : MonoBehaviour
    {
        public TowerExternalData Data;
        public CableGroups CableGroups;
        public FloorGroups FloorGroups;

        private void OnEnable()
        {
            GeneralEventbus.OnTowersAndTeamsReady += Initialize;
        }

        private void Initialize()
        {
            GetElements();
            CreateAndSetGroups();
        }

        void GetElements()
        {
            Data.Cables = GetComponentsInChildren<Cable>();
            Data.Floors = GetComponentsInChildren<Floor>();
        }

        void CreateAndSetGroups()
        {
            CableGroups = new CableGroups(Data.Cables);
            FloorGroups = new FloorGroups(Data.Floors);
            
            CableGroups.SetColor(Data.CableSelectionColor, Data.CableDefaultColor);
            SubscribeToGroups();
        }

        void SubscribeToGroups()
        {
            CableGroups.Subscribe();
            FloorGroups.Subscribe();
        }

        void UnsubscribeFromGroups()
        {
            CableGroups.Unsubscribe();
            FloorGroups.Unsubscribe();
        }

        private void OnDisable()
        {
            GeneralEventbus.OnTowersAndTeamsReady -= Initialize;
            UnsubscribeFromGroups();
        }
    }
}