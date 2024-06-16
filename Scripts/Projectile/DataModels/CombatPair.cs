using System;
using System.Threading.Tasks;
using GameUI;
using UnityEngine;
using ProjectileHandler;
using Towers;
using Object = UnityEngine.Object;

namespace DataModels
{
    public static class CombatPairEvents
    {
        public static Action<CombatPair> OnShoot;
    }
   
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
                    CombatPairEvents.OnShoot?.Invoke(this);
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
        

        void SkipCombat()
        {
            CompleteCombat();
        }

        public void CompleteCombat()
        {
            CombatCompleted = true;
        }
    }
}