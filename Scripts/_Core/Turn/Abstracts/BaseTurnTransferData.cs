using System;
using System.Collections.Generic;
using Towers;
using UnityEngine;


namespace Turn
{
    [Serializable]
    public abstract class BaseTurnTransferData
    {
        public abstract List<int> Towers { get; set; }
        
        public void ResetPreviousTurnData()
        {
            Towers.Clear();
        }
        
        public void RestorePreviousSelectionColors()
        {
            Towers.ForEach(s=>AllTowers.GetTower(s).SelectColor());
            Debug.Log("restore colors");
        }

    }
}

