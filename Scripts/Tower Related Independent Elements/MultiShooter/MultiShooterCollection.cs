using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerRelated
{
    public class MultiShooterCollection : TowerRelatedElementCollection<MultiShooter>
    {
       

        public override void Subscribe()
        {
            Eventbus.TowerEvents.OnMultiShotActionTriggered += RevealMultiShot;
        }

        public override void Initialize()
        {
            
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
