using UnityEngine;
using ProjectileHandler;
using Towers;
using UI;

namespace DataModels
{
    public class CombatPair
    {
        public Tower Perpetrator { get; }
        public Tower Victim { get; }
        public bool IsEven { get; }
        public bool CombatCompleted { get; private set; } = false;
        

        public CombatPair(Tower _perpetrator, Tower _victim, bool isEven = false)
        {
            Perpetrator = _perpetrator;
            Victim = _victim;
            IsEven = isEven;
        }

        public bool Contains(int newTower)
        {
            return Perpetrator.Data.UniqID == newTower || Victim.Data.UniqID == newTower;
        }

        public void Combat(float duration)
        {
            if (IsEven)
            {
                CombatCompleted = true;
                return;
            }
            
            if (Perpetrator.Data.Health <= 0) return; //INFO: runtime esnasında ölmüş olabilir //match runtime'a alındığı için buna gerek olmayabilir
            SendProjectile(duration);
        }

        void SendProjectile(float duration)
        {
            var projectile = ProjectilePool.Instance.GetItem(p => p.transform.position = Perpetrator.towerParts.Data.Top.transform.position);
            projectile.Setup(duration, Victim.towerParts.Data.Top.transform.position-Vector3.up);
            projectile.Move(OnComplete);
        }

        void OnComplete()
        {
            Victim.Data.Health -= Perpetrator.ConstantData.DamagePower;
            UIEventbus.OnHealthChange.Invoke(Victim.Data.Health, Victim.gameObject);
            
            if (Victim.Data.Health <= 0)
                Eventbus.CombatEvents.OnTowerKilled?.Invoke(Victim);

            CombatCompleted = true;
        }
        
    }
}