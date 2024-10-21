using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Actor
{
    public class RelationEmployee : ActorEmployee
    {
        public RelationEmployee(ActorHolder holder) : base(holder) {}
        
        public override void Subscribe()
        {
            Eventbus.LinkEvents.OnCreatingCombatPairs += SetLinkedActors;
        }
        
        public static void SetLinkedActors(List<uint> actors) //ters de gelebilir
        {
            
            ResetAllLinks();
            var actorsAmount = actors.Count;
            for (var i = 0; i < actorsAmount ; i++)
            {
                var mainID = actors[i];
                var nextIDInOrder = actors[(i + 1) % actorsAmount];
//                Debug.Log(ActorHolder.Registry[mainID].Row);
                //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da
                
                ActorHolder.Registry[mainID].LinkActors(nextIDInOrder); //burda patlar, double'ın elemanı olup registeryde bulunmayabilir!
            }
        }

        public static void SetNeighbours(List<uint> actors)
        {
            var actorsAmount = actors.Count;
            for (var i = 0; i < actorsAmount ; i++)
            {
                var mainID = actors[i];
                var mainActor = ActorHolder.Registry[mainID];

                uint previousID = actors[i - 1];
                if (i - 1 < 0)
                {
                    previousID = actors[actorsAmount - 1];
                }
                
                var nextID = actors[(i + 1) % actorsAmount];
                
                mainActor.SetNeighbours(previousID, nextID);
                
            }
            //----
            // for (var i = 0; i < TowersCount; i++)
            // {
            //     TowerDatas[i].NeighbourIDs.Clear();
            //     
            //     int previousID = i - 1;
            //     if (previousID < 0)
            //         previousID = TowersCount - 1;
            //     int previous =  TowerDatas[previousID].UniqID;
            //     
            //     int next = TowerDatas[(i + 1) % TowersCount].UniqID;
            //     
            //     TowerDatas[i].NeighbourIDs.Add(previous);
            //     TowerDatas[i].NeighbourIDs.Add(next);
            // }
        }

        private static void ResetAllLinks()
        {
            foreach (var registry in ActorHolder.Registry.Values)
            {
                registry.LinkedActors.Clear();
            }
        }

        public override void Unsubscribe()
        {
            Eventbus.LinkEvents.OnCreatingCombatPairs -= SetLinkedActors;
        }

    }
}