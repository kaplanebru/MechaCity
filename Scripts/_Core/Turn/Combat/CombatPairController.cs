using System;
using System.Collections.Generic;
using System.Linq;
using Actor;
using DataModels;
using Enums.Combat;
using Towers;
using UnityEngine;

namespace Turn
{
 
    public class CombatPairController
    {
        private static  Dictionary<uint, List<CombatPair>> pairGroupsByActor = new();
        private Dictionary<int, CombatPair> allPairs = new();
        private CombatPairsCreator combatPairsCreator = new();
        
        public void Subscribe()
        {
            Eventbus.ActorEvents.OnGridRegistrySet += SetCombatPairs;
            Eventbus.CombatEvents.OnCombatCompleteRequest += CompleteCombatForPair;
        }

        private void CompleteCombatForPair(int pairID)
        {
            allPairs[pairID].CompleteCombat();
        }
        public CombatPair GetCombatPairByID(int pairID) => allPairs[pairID];
        public static List<CombatPair> GetPairGroupByActorID(uint actorID) => pairGroupsByActor[actorID];

        public int PairAmount => pairGroupsByActor.Count;

        public void ResetCombatCompletedForAll()
        {
            foreach (var pairs in allPairs.Values)
            {
                pairs.CombatCompleted = false;
            }
        }

        private void SetCombatPairs(List<uint> actors, bool isReversed)
        {
            var tuple = combatPairsCreator.CreateCombatPairs(actors, isReversed);
            
            pairGroupsByActor = tuple.Item1;
            allPairs = tuple.Item2;
            
            SetLiaisons();
            Eventbus.CombatEvents.OnPairsSet?.Invoke(isReversed);
        }
        
        void SetLiaisons()
        {
            foreach (var actor in ActorDB.Registry.Values)
            {
                if(actor.TargetActors.Count <= 1) continue;
                var pairs = GetPairGroupByActorID(actor.ID);
                
                pairs.ForEach(p=>p.liaisonStatus = LiaisonStatus.OnBoth);
                pairs.First().liaisonStatus = LiaisonStatus.OnEnd;
                pairs.Last().liaisonStatus = LiaisonStatus.OnStart;
            }
        }
        
        public void Unsubscribe()
        {
            Eventbus.ActorEvents.OnGridRegistrySet -= SetCombatPairs;
            Eventbus.CombatEvents.OnCombatCompleteRequest -= CompleteCombatForPair;
        }

    }

}
