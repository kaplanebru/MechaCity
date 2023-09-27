using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Models
{
    public class CombatPair
    {
        public Tower Perpetrator { get; }
        public Tower Victim { get; }

        public bool IsEven { get; }
        
        public bool Dead { get; private set; }

        public CombatPair(Tower _perpetrator, Tower _victim, bool isEven = false)
        {
            Dead = false;
            Perpetrator = _perpetrator;
            Victim = _victim;
            IsEven = isEven;
        }

        public bool Contains(Tower newTower)
        {
            return Perpetrator == newTower || Victim == newTower;
        }

        public void Combat(float speed)
        {
            if (IsEven) return;
            if (Perpetrator.Data.Health <= 0) return; //INFO: runtime esnasında ölmüş olabilir //match runtime'a alındığı için buna gerek olmayabilir
           
            //Victim.Descend(Perpetrator.ConstantData.DamagePower);

            var projectile = ProjectilePool.Instance.GetItem(p => p.transform.position = Perpetrator.towerParts.Data.Top.transform.position);
            projectile.Setup(speed, Victim.towerParts.Data.Top.transform.position-Vector3.up);
            projectile.Move(RemoveVictimHealth);
        }

        void RemoveVictimHealth()
        {
            if (Victim.Data.Health <= 0) return;

            Victim.Data.Health -= Perpetrator.ConstantData.DamagePower;
            Eventbus.UIEvents.OnHealthChange.Invoke(Victim.Data.Health, Victim);
            
            CheckVictimLife();
        }

        void CheckVictimLife()
        {
            if (Victim.Data.Health <= 0)
            {
                //Victim.SetColor(Victim.TransferData.TeamTowerData.DeadMaterial); //for debugging
                Dead = true;
                Eventbus.FireEvents.OnTowerKilled?.Invoke(Victim);
            }
        }
    }
}