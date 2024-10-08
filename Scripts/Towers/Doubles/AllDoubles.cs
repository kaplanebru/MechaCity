using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Towers;
using UnityEngine;

public class AllDoubles : MonoBehaviour
{
    private static List<DoubleTower> _doubles = new();
    public static IEnumerable Doubles => _doubles;

    public static void Add(DoubleTower doubleTower)
    {
        _doubles.Add(doubleTower);
    }

    public static void Remove(DoubleTower doubleTower)
    {
        _doubles.Remove(doubleTower);
    }

   
    public static bool TryInspectByTowerAndGetDouble(int id, out DoubleTower doubleTower)
    {
        foreach (var _double in _doubles)
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
    
    public static bool InspectByTower(int id)
    {
        return _doubles.Any(_double => _double.InspectByTowerID(id)); //_double.towers.ContainsKey(id)
    }
    
}
