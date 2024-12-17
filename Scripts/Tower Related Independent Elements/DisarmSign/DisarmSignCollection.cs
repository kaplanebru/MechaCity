using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerRelated
{
    public class DisarmSignCollection : TowerRelatedElementCollection<DisarmSign> 
    {
        
        public override void Subscribe()
        {
            Eventbus.TowerEvents.OnDisarmamentActionTriggered += RevealSign;
        }

        public override void Initialize()
        {
            
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
