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
            Data.Gears = GetComponentsInChildren<IGear>().ToList();
          
            for (int i = Data.Gears.Count - 1; i >= 0; i--)
            {
                var tagg = Data.Gears[i].GameObject.tag;
                if (tagg == "Cosmetic")
                {
                    Data.Gears.Remove(Data.Gears[i]);
                }
            }
        }

        void CreateAndSetGroups()
        {
            CableGroups = new CableGroups(Data.Cables);
            FloorGroups = new FloorGroups(Data.Floors);
            GearGroup = new GearGroup(Data.Gears.ToArray());
            
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
        }

        void UnsubscribeFromGroups()
        {
            CableGroups.Unsubscribe();
            FloorGroups.Unsubscribe();
            GearGroup.Unsubscribe();
        }

        private void OnDisable()
        {
            GeneralEventbus.InitializerEvents.OnTowersAndTeamsReady -= Initialize;
            UnsubscribeFromGroups();
        }
    }
}