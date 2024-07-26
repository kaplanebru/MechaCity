using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class GearGroup 
    {
        [SerializeField]private GearIdentifier[] _group;

        public GearGroup(GearIdentifier[] group)
        {
            _group = group;
        }

        public void Subscribe()
        {
            CommunEventbus.EffectEvents.OnDeathEffect += Rotate;
            GeneralEventbus.InitializerEvents.OnExternalElementsReady += RotateAll;
        }

        private void RotateAll()
        {
            foreach (var gear in _group)
            {
                gear.Rotate(90);
            }
        }

        private void Rotate(int id)
        {
            _group.FirstOrDefault(g=>g.Id == id)?.Rotate(360);
        }

        public void Unsubscribe()
        {
            CommunEventbus.EffectEvents.OnDeathEffect -= Rotate;
            GeneralEventbus.InitializerEvents.OnExternalElementsReady -= RotateAll;
        }

        void SendGears()
        {
            //CommunEventbus.SetupEvents.OnGearsReady?.Invoke(_group);
        }
    }
}

