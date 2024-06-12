using System;
using System.Threading.Tasks;
using UnityEngine;
using ProjectileHandler;
using Towers;
using GameUI;
using UnityEditor.Timeline.Actions;
using Object = UnityEngine.Object;

namespace DataModels
{
    public class CombatPair
    {
        public TowerData MainTowerData { get; }
        public TowerData OtherTowerData { get; }

        // private Tower _mainTower;
        // private Tower _nextTower;

        public bool CombatCompleted { get; set; } = false;

        public CombatPair(TowerData mainTowerData, TowerData otherTowerData)
        {
            MainTowerData = mainTowerData;
            OtherTowerData = otherTowerData;

            // _mainTower = AllTowers.GetTower(MainTowerData.UniqID);
            // _nextTower = AllTowers.GetTower(OtherTowerData.UniqID);
        }

        public bool Contains(int newTower)
        {
            return OtherTowerData.UniqID == newTower || MainTowerData.UniqID == newTower;
        }

        public bool Combat()
        {
            if (OtherTowerData.TeamType == MainTowerData.TeamType)
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
                    GeneralEventbus.OnShoot?.Invoke(MainTowerData.UniqID); //direkt pair de yollanabilir.
                    SendProjectile(MainTowerData, OtherTowerData, 1); //timingData.shootDuration
                    //TODO: invoke repeating: shoot duration + shooter duration, combat
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

        void SendProjectile(TowerData perpetrator, TowerData victim, float duration)
        {
            var projectile = ProjectilePool.Instance.GetItem(p =>
                p.transform.position = perpetrator.Mover.Data.Top.transform.position);
            projectile.Setup(duration, victim.Mover.Data.Top.transform.position - Vector3.up * .5f); //-Vector3.up

            perpetrator.BulletAmount--;

            projectile.Move(() =>
            {
                perpetrator.ColorHandler.ToOriginalColor();
                RemoveHealth(victim);
            });
        }

        void RemoveHealth(TowerData victimData)
        {
            victimData.Health -= OtherTowerData.DamagePower;
            UIEventbus.OnHealthChange.Invoke(victimData.Health, OtherTowerData.UniqID);
            
            victimData.Mover.Shake();

            if(IsVictimDead(victimData,  AllTowers.GetTower(victimData.UniqID)))
                return;
            
            CompleteCombat();
        }

        bool IsVictimDead(TowerData victimData, Tower victim)
        {
            if (victimData.Health <= 0)
            {
                victim.HandleDeath(() =>
                        Eventbus.CombatEvents.OnTowerKilled?.Invoke(victimData.UniqID),
                    CompleteCombat);
                return true;
            }
            return false;
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