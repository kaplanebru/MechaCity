using System.Collections;
using System.Collections.Generic;
using Towers;
using UnityEngine;

namespace Blueprint
{
    public class BpTowerData
    {
        private int Id;
        public bool IsFreezing
        {
            set => AllTowers.GetData(Id).IsClickable = !value;
        }
        public bool IsDouble = false;

        public BpTowerData(int id)
        {
            Id = id;
        }
    }
}
