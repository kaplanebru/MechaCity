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
            _towerParts.SetColor(Data.TeamTowerData.FreezeMaterial);
        }

        public void ToBlueprintColor()
        {
            _towerParts.SetColor(Data.TeamTowerData.BlueprintMaterial);
        }

        public void ToSelectionColor()
        {
            _towerParts.SetColor(Data.TeamTowerData.SelectedMaterial);
        }

        public void ToOriginalColor()
        {
            _towerParts.SetColor(Data.TeamTowerData.DefaultMaterial);
        }

        public void FadeToDeadColor()
        {
            //_towerParts.FadeColor(Data.TeamTowerData.DeadMaterial);
        }

        public void ToDeadColor()
        {
            _towerParts.SetColor(Data.TeamTowerData.DeadMaterial);
        }

        public void ToRegenerationColor()
        {
            _towerParts.SetColor(Data.TeamTowerData.RegenerationMaterial);
        }
    }

}
