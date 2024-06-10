using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Towers
{
    public class TowerColorHandler
    {
        private TowerParts _towerParts;
        private TowerData Data;
        public TowerColorHandler(TowerData data, TowerParts towerParts)
        {
            Data = data;
            _towerParts = towerParts;
        }
        
        public void ToFreezeColor()
        {
            _towerParts.SetMats(Data.TeamTowerData.FreezeMaterial);
        }

        public void ToBlueprintColor()
        {
            _towerParts.SetMats(Data.TeamTowerData.BlueprintMaterial);
        }

        public void ToSelectionColor()
        {
            _towerParts.SetMats(Data.TeamTowerData.SelectedMaterial);
        }

        public void ToOriginalColor()
        {
            _towerParts.SetMats(Data.TeamTowerData.DefaultMaterial);
        }

        public void FadeToDeadColor()
        {
            //_towerParts.FadeColor(Data.TeamTowerData.DeadMaterial);
        }

        public void ToDeadColor()
        {
            _towerParts.SetMats(Data.TeamTowerData.DeadMaterial);
        }

        public void ToRegenerationColor()
        {
            _towerParts.SetMats(Data.TeamTowerData.RegenerationMaterial);
        }
        
        // public void ToGivenColor(Material givenMat)
        // {
        //     towerParts.SetColor(givenMat);
        // }
    }

}
