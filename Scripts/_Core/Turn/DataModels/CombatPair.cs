using System;
using System.Threading.Tasks;
using UnityEngine;
using ProjectileHandler;
using Towers;
using GameUI;

namespace DataModels
{
    public class CombatPair
    {
        public TowerData MainTowerData { get; }
        public TowerData OtherTowerData { get; }

        private Tower _mainTower;
        private Tower _nextTower;
       
        public bool CombatCompleted { get; private set; } = false;
        public CombatPair(TowerData mainTowerData, TowerData otherTowerData)
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

        public bool Combat()
        {
            if (OtherTowerData.TeamTowerData.TeamType == MainTowerData.TeamTowerData.TeamType)
            {
                SkipCombat();
                return false;
            }

            if (OtherTowerData.Health <= 0 || MainTowerData.Health <= 0)
            {
                SkipCombat();
                return false;
            }

            if (MainTowerData.Height > OtherTowerData.Height)
            {
                if (MainTowerData.CanShoot)
                {
                    SendProjectile(_mainTower, _nextTower, 1); //timingData.shootDuration
                    return true;
                }
               
                return false;
            }
            else
            {
               SkipCombat();
               return false;
            }
        }

        void SendProjectile(Tower perpetrator, Tower victim, float duration)
        {
            var projectile = ProjectilePool.Instance.GetItem(p => p.transform.position = perpetrator.towerParts.Data.Top.transform.position);
            projectile.Setup(duration, victim.towerParts.Data.Top.transform.position-Vector3.up *.5f); //-Vector3.up

            perpetrator.Data.BulletAmount--;
            
            projectile.Move(()=>RemoveHealth(victim.Data));
        }

        void RemoveHealth(TowerData victimData)
        {
            victimData.Health -= OtherTowerData.DamagePower;
            UIEventbus.OnHealthChange.Invoke(victimData.Health, _nextTower.gameObject);
            AllTowers.GetTower(victimData.UniqID).towerParts.Shake();

            if (victimData.Health <= 0)
            {
                AllTowers.GetTower(victimData.UniqID).HandleDeath();
                //Eventbus.CombatEvents.OnTowerKilled?.Invoke(victimData);
            }

            CompleteCombat();
        }

        void SkipCombat()
        {
            CompleteCombat();
        }

        void CompleteCombat()
        {
            CombatCompleted = true;
        }
        
    }
}