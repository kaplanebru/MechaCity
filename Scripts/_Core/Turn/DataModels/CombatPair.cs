using Data;
using UnityEngine;
using ProjectileHandler;
using Towers;
using UI;

namespace DataModels
{
    public class CombatPair
    {
        public TowerData Perpetrator { get; }
        public TowerData Victim { get; }

        public Tower PerpetratorObj;
        public Tower VictimObj;
        public bool IsEven { get; }
        public bool CombatCompleted { get; private set; } = false;
        

        public CombatPair(TowerData _perpetrator, TowerData _victim, bool isEven = false)
        {
            Perpetrator = _perpetrator;
            Victim = _victim;
            IsEven = isEven;

            PerpetratorObj = AllTowers.GetTower(Perpetrator.UniqID);
            VictimObj = AllTowers.GetTower(Victim.UniqID);

        }

        public bool Contains(int newTower)
        {
            return Perpetrator.UniqID == newTower || Victim.UniqID == newTower;
        }

        public void Combat(float duration)
        {
            if (IsEven)
            {
                CombatCompleted = true;
                return;
            }
            
            if (Perpetrator.Health <= 0) return; //INFO: runtime esnasında ölmüş olabilir //match runtime'a alındığı için buna gerek olmayabilir
            SendProjectile(duration);
        }

        void SendProjectile(float duration)
        {
            var projectile = ProjectilePool.Instance.GetItem(p => p.transform.position = PerpetratorObj.towerParts.Data.Top.transform.position);
            projectile.Setup(duration, VictimObj.towerParts.Data.Top.transform.position-Vector3.up);
            projectile.Move(OnComplete);
        }

        void OnComplete()
        {
            Victim.Health -= Perpetrator.DamagePower;
            UIEventbus.OnHealthChange.Invoke(Victim.Health, VictimObj.gameObject);
            
            if (Victim.Health <= 0)
                Eventbus.CombatEvents.OnTowerKilled?.Invoke(Victim);

            CombatCompleted = true;
        }
        
    }
}