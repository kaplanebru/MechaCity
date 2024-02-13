using UnityEngine;
using ProjectileHandler;
using Towers;
using GameUI;

namespace DataModels
{
    public class CombatGroup
    {
        public TowerData MainTowerData { get; }
        public TowerData OtherTowerData { get; }

        private Tower _mainTower;
        private Tower _nextTower;
       
        public bool CombatCompleted { get; private set; } = false;
        public CombatGroup(TowerData mainTowerData, TowerData otherTowerData)
        {
            MainTowerData = mainTowerData;
            OtherTowerData = otherTowerData;
            
            _mainTower = AllTowers.GetTower(MainTowerData.UniqID);
            _nextTower = AllTowers.GetTower(OtherTowerData.UniqID);
        }

        public bool Contains(int newTower)
        {
            return OtherTowerData.UniqID == newTower || MainTowerData.UniqID == newTower;
        }

        public void Combat(float duration)
        {
            if (OtherTowerData.Health <= 0 || MainTowerData.Health <= 0) return;

            if (MainTowerData.Height > OtherTowerData.Height)
            {
                if(MainTowerData.CanShoot)
                    SendProjectile(_mainTower, _nextTower, duration);
            }
            
            else if (OtherTowerData.Height > MainTowerData.Height)
            {
                if(OtherTowerData.CanShoot)
                    SendProjectile(_nextTower, _mainTower, duration);
            }
            
            else
                CombatCompleted = true;
        }

        void SendProjectile(Tower perpetrator, Tower victim, float duration)
        {
            var projectile = ProjectilePool.Instance.GetItem(p => p.transform.position = perpetrator.towerParts.Data.Top.transform.position);
            projectile.Setup(duration, victim.towerParts.Data.Top.transform.position-Vector3.up);

            perpetrator.Data.BulletAmount--;
            projectile.Move(()=>RemoveHealth(victim.Data));
            //sırf pozisyon için tower taşıma, dataya ekle!
        }

        void RemoveHealth(TowerData victimData)
        {
            victimData.Health -= OtherTowerData.DamagePower;
            UIEventbus.OnHealthChange.Invoke(victimData.Health, _nextTower.gameObject);
            
            if (victimData.Health <= 0)
                Eventbus.CombatEvents.OnTowerKilled?.Invoke(victimData);

            CombatCompleted = true;
        }
        
    }
}