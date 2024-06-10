using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using DG.Tweening;
using UnityEngine;



namespace Towers
{
    [Serializable]
    public class TowerVisualData
    {
        public TeamTowerData TeamData;
        public SpriteRenderer Logo;
        public MeshRenderer[] MiddleMeshes;
    }
    public class TowerColors : MonoBehaviour
    {
        public TowerVisualData Data;
        public CombatTimingData timingData;
        private ColorChanger colorChanger;
        public void Initialize()
        {
            colorChanger = new ColorChanger(timingData);
        }
       
        public void SetTeamVisuals(TeamTowerData teamData)
        {
            Data.TeamData = teamData;
            FadeColor();
            SetTeamLogo();
        }
        
        void SetTeamLogo()
        {
            
            Data.Logo.transform.DOScale(Vector3.zero, timingData.colorFadeDuration / 2).
                OnComplete(() =>
            {
                Data.Logo.sprite = Data.TeamData.TeamSprite;
                //Data.Logo.color = Data.TeamData.SpriteColor;
               
                Data.Logo.transform.DOScale(Vector3.one, timingData.colorFadeDuration / 2);
            });
        }
        
        void FadeColor()
        {
            colorChanger.FadeColors(Data.MiddleMeshes, Data.TeamData.TeamColors);
        }
        
        public void SetMats(Material[] mats)
        {
            colorChanger.SetMats(Data.MiddleMeshes, mats);
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
