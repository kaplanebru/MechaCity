using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class TowerExternalElementsHolder : MonoBehaviour
    {
        public TowerExternalData Data;
        public CableGroups CableGroups;
        public FloorGroups FloorGroups;
        public GearGroup GearGroup;
        public ShieldGroup ShieldGroup;
        public MultiShooterGroup MultiShooterGroup;

        private void OnEnable()
        {
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady += Initialize;
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
            Data.IGears = GetComponentsInChildren<IGear>();
            Data.Shields = GetComponentsInChildren<Shield>();
            Data.MultiShooters = GetComponentsInChildren<MultiShooter>();
        }

        void CreateAndSetGroups()
        {
            CableGroups = new CableGroups(Data.Cables);
            FloorGroups = new FloorGroups(Data.Floors);
            GearGroup = new GearGroup(Data.IGears.ToArray());
            ShieldGroup = new ShieldGroup(Data.Shields);
            MultiShooterGroup = new MultiShooterGroup(Data.MultiShooters);
            
            
            CableGroups.SetColor(Data.CableSelectionColor, Data.CableDefaultColor);
            SubscribeToGroups();
            ReadyCall();
        }

        void ReadyCall()
        {
            GeneralEventbus.InitializerEvents.OnExternalElementsReady?.Invoke();
        }

        void SubscribeToGroups()
        {
            CableGroups.Subscribe();
            FloorGroups.Subscribe();
            GearGroup.Subscribe();
            ShieldGroup.Subscribe();
            MultiShooterGroup.Subscribe();
        }

        void UnsubscribeFromGroups()
        {
            CableGroups.Unsubscribe();
            FloorGroups.Unsubscribe();
            GearGroup.Unsubscribe();
            ShieldGroup.Unsubscribe();
            MultiShooterGroup.Unsubscribe();
        }

        private void OnDisable()
        {
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady -= Initialize;
            UnsubscribeFromGroups();
        }
    }
}