using System;
using System.Collections.Generic;
using UnityEngine;

namespace TowerExternal
{
    [Serializable]
    public class TowerRelatedElementDataBase
    {
        public Dictionary<TowerRelatedType, ITowerRelatedElement[]> TowerRelatedElements = new();
        
        
        public Floor[] Floors;
        public IGear[] IGears;
        public Shield[] Shields;
        public MultiShooter[] MultiShooters;
        public DisarmSign[] DisarmSigns;

        public List<Floor> floors = new();


        public void Fill()
        {
            TowerRelatedElements.Add(TowerRelatedType.Floor, Floors); //new Floor[]{}
        }
      
    }

   
}

