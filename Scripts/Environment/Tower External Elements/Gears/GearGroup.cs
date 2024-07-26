using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class GearGroup 
    {
        [SerializeField]private IGear[] _group;
        [SerializeField] private List<GearIdentifier> _gearIdentifiers = new();

        public GearGroup(IGear[] group)
        {
            _group = group;
            Setup();
        }

        void Setup()
        {
            GetGearIdentifiers();
            MediatorEventbus.SetupEvents.OnGearsReady?.Invoke(_group);
        }

        void GetGearIdentifiers()
        {
            foreach (var gear in _group)
            {
                _gearIdentifiers.Add(gear.GameObject.GetComponent<GearIdentifier>());
            }
        }

        public void Subscribe()
        {
            MediatorEventbus.EffectEvents.OnDeathEffect += Rotate;
            GeneralEventbus.InitializerEvents.OnExternalElementsReady += RotateAll;
        }

        private void RotateAll()
        {
            foreach (var gear in _gearIdentifiers)
            {
                gear.Rotate(90);
            }
        }

        private void Rotate(int id)
        {
            _gearIdentifiers.FirstOrDefault(g=>g.Id == id)?.Rotate(360);
        }

        public void Unsubscribe()
        {
            MediatorEventbus.EffectEvents.OnDeathEffect -= Rotate;
            GeneralEventbus.InitializerEvents.OnExternalElementsReady -= RotateAll;
        }
        
    }
}

