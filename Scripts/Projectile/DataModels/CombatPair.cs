using System;
using System.Linq;
using System.Threading.Tasks;
using Actor;
using Enums.Combat;
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
        public int ID;
        public ActorData MainActor;
        public ActorData OtherActor;
        public TowerNumericData MainTowerData { get; private set; }
        public TowerNumericData OtherTowerData { get; private set; }

        public bool CombatCompleted { get; set; } = false;
        public Enums.Combat.LiaisonStatus liaisonStatus = LiaisonStatus.None;

        public CombatPair(ActorData mainActor, ActorData otherActor)
        {
            MainActor = mainActor;
            OtherActor = otherActor;
        }

        public void OrderTowersByGridDirection(bool isReversed)
        {
            if (!isReversed)
            {
                MainTowerData =
                    MainActor.TowerNumericDatas.OrderBy(t => t.UniqID)
                        .Last(); //AllTowers.GetData(MainActor.TowerIDs.Last());
                OtherTowerData =
                    OtherActor.TowerNumericDatas.OrderBy(t => t.UniqID)
                        .First(); //AllTowers.GetData(OtherActor.TowerIDs.First());
            }
            else
            {
                MainTowerData =
                    MainActor.TowerNumericDatas.OrderBy(t => t.UniqID)
                        .First(); //AllTowers.GetData(MainActor.TowerIDs.First());
                OtherTowerData =
                    OtherActor.TowerNumericDatas.OrderBy(t => t.UniqID)
                        .Last(); //AllTowers.GetData(OtherActor.TowerIDs.Last());
            }
        }

        public bool Combat()
        {
            if (OtherActor.TeamType == MainActor.TeamType)
                goto Skip;

            if (!MainActor.ActivityStatus.CanShoot)
                goto Skip;

            if (MainTowerData.Height > OtherTowerData.Height)
            {
                CombatPairEvents.OnShoot?.Invoke(this);
                return true;
            }

            Skip:
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

        public bool ContainsMainActor(uint actorID)
        {
            return MainActor.ID == actorID;
        }

        public bool Contains(int newTower)
        {
            return OtherTowerData.UniqID == newTower || MainTowerData.UniqID == newTower;
        }
    }
}