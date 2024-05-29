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
        public Dictionary<BpType, BaseBlueprint> AllBlueprints = new();

        public void Initialize()
        {
            CreateBlueprints();
        }
        void CreateBlueprints() //Burası ortadaki kısımla ilgili
        {
            AllBlueprints.Add(BpType.Reverse, new BpReverse());
            AllBlueprints.Add(BpType.Freeze, new BpFreeze());
            AllBlueprints.Add(BpType.SelectionIncrement, new BpSelectionIncrement());
            //AllBlueprints.Add(BpType.Double, new BpDouble());
        }
        
        
        
       

       

       
    }


   
}

