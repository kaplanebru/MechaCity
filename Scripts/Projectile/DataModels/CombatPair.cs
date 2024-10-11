using System;
using System.Threading.Tasks;
using Actor;
using GameUI;
using Health;
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

        public bool CombatCompleted { get; set; } = false;

        public CombatPair(TowerData mainTowerData, TowerData otherTowerData)
        {
            MainTowerData = mainTowerData;
            OtherTowerData = otherTowerData;
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

            //TODO: kendisi ya da other Double olabilir
            //EN BAŞTAN double ile pairler güncellenebilir. Ama height 2. bir check gerektirecektir.
            if(ActorManager.GetHealth(OtherTowerData.UniqID) <= 0 || ActorManager.GetHealth(MainTowerData.UniqID) <= 0)
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
            
            SkipCombat();
            return false;
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