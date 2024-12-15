using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerExternal
{
    public class MultiShooterGroup : BaseTowerExternalGroup<MultiShooter>
    {
        public MultiShooterGroup(MultiShooter[] group) : base(group)
        {
        }

        public override void Subscribe()
        {
            Eventbus.TowerEvents.OnMultiShotActionTriggered += RevealMultiShot;
        }

        private void RevealMultiShot(int towerID, int shooterAmount)//shooter amountu pas geçebiliriz
        {
            var multiShooter = Group[towerID];
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
