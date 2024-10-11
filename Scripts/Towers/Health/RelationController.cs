using System.Collections.Generic;
using Towers;

namespace Actor
{
    public class RelationController : ActorController
    {
        public RelationController(ActorManager manager) : base(manager) {}
        
        public override void Subscribe()
        {
                
        }

        public void SetRelationData(int id, params int[] towers)
        {
            ActorManager.Registry[id].LinkedTowers.Clear();
            ActorManager.Registry[id].LinkedTowers.AddRange(towers);
        }

        public static void SetLinkedTowers(List<TowerData> towers) //ters de gelebilir
        {
            ResetAllLinks();
            //link de dahil gibi düşün, ters çevirdiğinde dümdüz ters çevirmek sorun olabilir
            for (var i = 0; i < AllTowers.TowersCount; i++)
            {
                var mainID = towers[i].UniqID;
                int next = towers[(i + 1) % AllTowers.TowersCount].UniqID; //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da
                
              
                //ActorManager.Registry.Add(mainID, new RelationData(next));
               
            }
        }

        private static void ResetAllLinks()
        {
            // foreach (var relation in Relations.Values)
            // {
            //     relation.LinkedTowers.Clear();
            // }
        }

        public override void Unsubscribe()
        {
                
        }

    }
}