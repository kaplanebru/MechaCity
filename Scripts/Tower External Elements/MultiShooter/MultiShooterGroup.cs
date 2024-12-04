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

        public void Subscribe()
        {
            Eventbus.TowerEvents.OnMultiShotActionTriggered += RevealMultiShot;
        }

        private void RevealMultiShot(int towerID, int shooterAmount)//shooter amountu pas geçebiliriz
        {
            
        }

        public void Unsubscribe()
        {
            Eventbus.TowerEvents.OnMultiShotActionTriggered -= RevealMultiShot;
        }
    }

}
