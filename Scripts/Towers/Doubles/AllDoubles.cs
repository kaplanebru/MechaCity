using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Towers;
using UnityEngine;

public class AllDoubles
{
    private static List<DoubleTower> Doubles  = new();
    public static Dictionary<int, DoubleTower> DoublesByID  { get; private set; }  = new();

    public static void Add(DoubleTower doubleTower)
    {
        Doubles.Add(doubleTower);
        DoublesByID.Add(doubleTower.ID, doubleTower);
    }

    public static void Remove(DoubleTower doubleTower)
    {
        Doubles.Remove(doubleTower);
        DoublesByID.Remove(doubleTower.ID);
    }

   
   
    public static bool TryInspectTowerAndGetDouble(int id, out DoubleTower doubleTower)
    {
        foreach (var _double in Doubles)
        {
            if (_double.InspectByTowerID(id)) //.towers.ContainsKey(id)
            {
                doubleTower = _double;
                return true;
            }
        }

        doubleTower = null;
        return false;
    }
    
    // public static bool InspectTower(int id)
    // {
    //     return Doubles.Any(_double => _double.InspectByTowerID(id)); //_double.towers.ContainsKey(id)
    // }
    //
    // public static DoubleTower TryGetDoubleByID(int id)
    // {
    //     foreach (var key in DoublesByID.Keys)
    //     {
    //         if (key == id)
    //             return DoublesByID[id];
    //     }
    //     
    //     return null;
    // }
    
    
}
