using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerExternal
{
    public class DisarmSignCollection : BaseTowerRelatedCollection<DisarmSign>
    {
        public DisarmSignCollection(DisarmSign[] collection) : base(collection)
        {
        }


        public override void Subscribe()
        {
            Eventbus.TowerEvents.OnDisarmamentActionTriggered += RevealSign;
        }

        private void RevealSign(int towerID)
        {
            var sign = Collection[towerID];
            sign.RevealSign();
        }

        private void HideSign(int towerID)
        {
            var sign = Collection[towerID];
            sign.HideSign();
        }

        public override void Unsubscribe()
        {
            Eventbus.TowerEvents.OnDisarmamentActionTriggered -= RevealSign;
        }
    }

}
