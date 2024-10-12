using System;
using System.Linq;
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
        public ActorData MainActor;
        public ActorData OtherActor;
        public TowerData MainTowerData { get; }
        public TowerData OtherTowerData { get; }

        public bool CombatCompleted { get; set; } = false;

        public CombatPair(ActorData mainActor, ActorData otherActor)//(TowerData mainTowerData, TowerData otherTowerData)
        {
            MainActor = mainActor;
            OtherActor = otherActor;
            
            MainTowerData = AllTowers.GetData(MainActor.Towers.Last());
            OtherTowerData = AllTowers.GetData(OtherActor.Towers.First());
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
            
            // if(ActorManager.GetHealth(OtherTowerData.UniqID) <= 0 || ActorManager.GetHealth(MainTowerData.UniqID) <= 0) //TODO: burda othertower double olabilir!! Böyle bir case yok bir yandan da!
            // {
            //     SkipCombat();
            //     return false;
            // }

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