using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using DG.Tweening;
using UnityEngine;



namespace Towers
{
    [Serializable]
    public class TowerColorData : TowerSegmentData
    {
        
        public TeamTowerData TeamData;
        public SpriteRenderer Logo;
        public MeshRenderer[] MiddleMeshes;
        public CombatTimingData TimingData;
    }
    public class ColorHandler : ITowerSegment
    {
        public int Id { get; set; }
        private TowerColorData Data;
        public ColorHandler(TowerSegmentData data)
        {
            Data = data as TowerColorData;
        }
        private ColorChanger colorChanger;
        
        public void SetId(int id)
        {
            Id = id;
        }
        public void Initialize()
        {
            colorChanger = new ColorChanger(Data.TimingData);
        }
       
        public void SetTeamVisuals(TeamTowerData teamData)
        {
            Data.TeamData = teamData;
            FadeColor();
            SetTeamLogo();
        }
        
        void SetTeamLogo()
        {
            
            Data.Logo.transform.DOScale(Vector3.zero, Data.TimingData.colorFadeDuration / 2).
                OnComplete(() =>
            {
                Data.Logo.sprite = Data.TeamData.TeamLogo;
                Data.Logo.color = Data.TeamData.LogoMat.color;
               
                Data.Logo.transform.DOScale(Vector3.one, Data.TimingData.colorFadeDuration / 2);
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
            GeneralEventbus.OnTurnTowerSelection?.Invoke(Id);
        }

        public void ToOriginalColor()
        {
            SetMats(Data.TeamData.DefaultMaterial);
            GeneralEventbus.OnTurnTowerDeselect?.Invoke(Id);
        }
        
    }

}
