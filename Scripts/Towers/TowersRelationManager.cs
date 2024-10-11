using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Towers
{

    public class RelationData
    {
        public List<int> LinkedTowers = new();

        public RelationData(params int[] towers)
        {
            foreach (var tower in towers)
            {
                LinkedTowers.Add(tower);
            }
            
        }
        public int Tallest;
        public int Lowest;
    }
    public class TowersRelationManager 
    {
        public static Dictionary<int, RelationData> Relations { get; private set; } = new ();
        

        //dizilime göre artarda oldukları varsayılıyor, id'ye göre değil!

        public static void SetLinkedTowers(List<TowerData> towers) //ters de gelebilir
        {
            ResetAllLinks();
            //link de dahil gibi düşün, ters çevirdiğinde dümdüz ters çevirmek sorun olabilir
            for (var i = 0; i < AllTowers.TowersCount; i++)
            {
                var mainID = towers[i].UniqID;
                int next = towers[(i + 1) % AllTowers.TowersCount].UniqID; //sonra gelenin id'sini alıyor, bu artan da olabilir azalan da

                //if(AllDoubles.InspectTower())
              
                Relations.Add(mainID, new RelationData(next));
                //double varsa i++ denip loop pas geçilebilir
            }
        }
        
        private static void ResetAllLinks()
        {
            foreach (var relation in Relations.Values)
            {
                relation.LinkedTowers.Clear();
            }
        }

    }
    
    // public void SetLinkedNeighbors()
    // {
    //     ResetAllNeighbours();
    //     for (var i = 0; i < AllTowers.TowersCount; i++)
    //     {
    //         AllTowers.TowerDatas[i].NeighbourIDs.Clear();
    //             
    //         int previousID = i - 1;
    //         if (previousID < 0)
    //             previousID = AllTowers.TowersCount - 1;
    //             
    //         int previous =  AllTowers.TowerDatas[previousID].UniqID;
    //         int next = AllTowers.TowerDatas[(i + 1) % AllTowers.TowersCount].UniqID;
    //             
    //         AllTowers.TowerDatas[i].NeighbourIDs.Add(previous);
    //         AllTowers.TowerDatas[i].NeighbourIDs.Add(next);
    //     }
    // }

}
