using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Network;
using UnityEngine;

namespace Blueprint
{
    public class BpHolder 
    {
        public static Dictionary<BpType, BaseBlueprint> AllBlueprints = new();
        public static void CreateBlueprints() //Burası ortadaki kısımla ilgili
        {
            AllBlueprints.Add(BpType.Reverse, new BpReverse());
            AllBlueprints.Add(BpType.Freeze, new BpFreeze());
            AllBlueprints.Add(BpType.SelectionIncrement, new BpSelectionIncrement());
            AllBlueprints.Add(BpType.DoubleSelf, new BpDoubleSelf());
            AllBlueprints.Add(BpType.Double, new BpDouble());
            AllBlueprints.Add(BpType.Shield, new BpShield());
        }
    }


   
}

