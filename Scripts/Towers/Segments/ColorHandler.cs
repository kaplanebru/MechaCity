using System;
using System.Collections;
using System.Collections.Generic;
using DataModels;
using DG.Tweening;
using Enums;
using Enums.Selections;
using UnityEngine;



namespace Towers
{
    [Serializable]
    public class TowerColorData : TowerSegmentData
    {
        
        public TeamTowerData TeamData;
        public SpriteRenderer Logo;
        public MeshCombiner[] MeshCombiners;
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
            SetCombinedMeshes();
            FadeColor();
            SetTeamLogo();
        }
        
        private void SetCombinedMeshes()
        {
            foreach (var meshCombiner in Data.MeshCombiners)
            {
                meshCombiner.CombineMeshes();
                meshCombiner.SetMaterial(Data.TeamData.CombinedMat[0]);
            }
        
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
        
        private void SetMats(Material[] mats)
        {
            colorChanger.SetMats(Data.MiddleMeshes, mats);
            foreach (var meshCombiner in Data.MeshCombiners)
            {
                meshCombiner.SetMaterial(Data.TeamData.CombinedMat[0]);
            }
            
        }
        
        public void SetColorByColorType(ColorType type)
        {
            SetMats(Data.TeamData.GetColorByType(type));
            GeneralEventbus.OnTowerColorChange?.Invoke(Id);
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
            GeneralEventbus.OnTowerColorChange?.Invoke(Id);
        }

        public void ToOriginalColor()
        {
            SetMats(Data.TeamData.DefaultMaterial);
            GeneralEventbus.OnTurnTowerDeselect?.Invoke(Id);
        }
        
    }

}
