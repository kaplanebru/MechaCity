using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using UnityEngine;



namespace Towers
{
    [Serializable]
    public class TowerVisualData
    {
        public TeamTowerData TeamData;
        public MeshRenderer[] MiddleMeshes;
    }
    public class TowerVisuals : MonoBehaviour
    {
        public TowerVisualData Data;
        public CombatTimingData timingData;
        private ColorChanger colorChanger;
        public void Initialize()
        {
            colorChanger = new ColorChanger(timingData);
        }
        public void SetMats(Material[] mats)
        {
            colorChanger.SetMats(Data.MiddleMeshes, mats);
        }

        public void FadeColor()
        {
            colorChanger.FadeColors(Data.MiddleMeshes, Data.TeamData.TeamColors);
            // Data.Sun.color = Color.cyan;
        }
        
        public void ToFreezeColor()
        {
            SetMats(Data.TeamData.FreezeMaterial);
        }

        public void ToBlueprintColor()
        {
            SetMats(Data.TeamData.BlueprintMaterial);
        }

        public void ToSelectionColor()
        {
            SetMats(Data.TeamData.SelectedMaterial);
        }

        public void ToOriginalColor()
        {
            SetMats(Data.TeamData.DefaultMaterial);
        }
    }

}
