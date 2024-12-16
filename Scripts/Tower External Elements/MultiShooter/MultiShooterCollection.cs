using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerExternal
{
    public class MultiShooterCollection : BaseTowerRelatedCollection<MultiShooter>
    {
        public MultiShooterCollection(MultiShooter[] collection) : base(collection)
        {
        }

        public override void Subscribe()
        {
            Eventbus.TowerEvents.OnMultiShotActionTriggered += RevealMultiShot;
        }

        private void RevealMultiShot(int towerID, int shooterAmount)//shooter amountu pas geçebiliriz
        {
            var multiShooter = Collection[towerID];
            if (shooterAmount == 2)
            {
                multiShooter.ShowShootingTable();
                multiShooter.RevealNewShooter(0);
            }
            else if(shooterAmount > 2)
            {
                multiShooter.RevealNewShooter(1);
            }
        }

        public override void Unsubscribe()
        {
            Eventbus.TowerEvents.OnMultiShotActionTriggered -= RevealMultiShot;
        }
    }

}
