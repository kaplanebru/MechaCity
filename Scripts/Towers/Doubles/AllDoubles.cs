using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Turn;
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

    public static bool InspectTower(int id)
    {
        return _doubles.Any(_double => _double.towers.ContainsKey(id));
    }
    
    public static DoubleTower GetDoubleByTower(int id)
    {
        return _doubles.FirstOrDefault(_double => _double.towers.ContainsKey(id));
    }

    public static bool InspectDouble(int id)
    {
        foreach (var _double in _doubles)
        {
            if (_double.towers.ContainsKey(id))
            {
                return true;
            }
        }
        return false;
    }
    
}
