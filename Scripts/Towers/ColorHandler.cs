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
        public int UniqId;
        public TeamTowerData TeamData;
        public SpriteRenderer Logo;
        public MeshRenderer[] MiddleMeshes;
    }
    public class ColorHandler : MonoBehaviour, ITowerSegment
    {
        public TowerVisualData Data;
        public CombatTimingData timingData;
        private ColorChanger colorChanger;

       

        public void SetId(int id)
        {
            Data.UniqId = id;
        }
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
                Data.Logo.sprite = Data.TeamData.TeamLogo;
                Data.Logo.color = Data.TeamData.LogoMat.color;
               
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
            GeneralEventbus.OnTurnTowerSelection?.Invoke(Data.UniqId);
        }

        public void ToOriginalColor()
        {
            SetMats(Data.TeamData.DefaultMaterial);
            GeneralEventbus.OnTurnTowerDeselect?.Invoke(Data.UniqId);
        }
        
    }

}
