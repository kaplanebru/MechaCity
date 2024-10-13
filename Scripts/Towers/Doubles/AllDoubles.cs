using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Towers
{
    public class AllDoubles
    {
        public static class DoubleTowerEvents
        {
            public static Func<int[], uint> OnDoubleTowerCreated;
        }
        private static Dictionary<uint, DoubleTower> DoublesByActorID { get; } = new();

        public static void RegisterDouble(uint actorID, DoubleTower doubleTower)
        {
            DoublesByActorID.Add(actorID, doubleTower);
        }

        public static void RemoveDouble(uint actorID)
        {
            DoublesByActorID.Remove(actorID); //todo: actorü yok etmeden önce double'ı yok etmek gerekir
        }

        public static DoubleTower GetDouble(uint actorID)
        {
            if (!DoublesByActorID.ContainsKey(actorID))
            {
                Debug.LogError("NO Double with this ACTOR ID");
                return null;
            }

            return DoublesByActorID[actorID];
        }
        
        
        // public static bool TryInspectTowerAndGetDouble(int id, out DoubleTower doubleTower)
        // {
        //     foreach (var _double in Doubles)
        //     {
        //         if (_double.InspectByTowerID(id)) //.towers.ContainsKey(id)
        //         {
        //             doubleTower = _double;
        //             return true;
        //         }
        //     }
        //
        //     doubleTower = null;
        //     return false;
        // }
    }
}