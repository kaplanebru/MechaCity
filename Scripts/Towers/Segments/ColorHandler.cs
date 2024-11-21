using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        
        public TeamColorData TeamData;
        public SpriteRenderer Logo;
        public MeshCombiner[] MeshCombiners;
        public MeshRenderer[] MiddleMeshes;
        public CombatTimingData TimingData;
    }
    public class ColorHandler : ITowerSegment
    {
        public int Id { get; set; }
        private TowerColorData Data;
        private List<MeshRenderer> combinedRenderers = new();
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
            SetCombinedMeshes();
        }
       
        public void SetTeamVisuals(TeamColorData teamData)
        {
            Data.TeamData = teamData;
            
            //FadeColor();
            //SetTeamLogo();
        }
        
        private void SetCombinedMeshes()
        {
            foreach (var meshCombiner in Data.MeshCombiners)
            {
                meshCombiner.CombineMeshes(out MeshRenderer renderer);
                combinedRenderers.Add(renderer);
            }
        }

        private void SetMats(params Material[] mats)
        {
            colorChanger.SetMaterial(mats[0], Data.MiddleMeshes);
            colorChanger.SetMaterial(mats[1],combinedRenderers.ToArray());
        }
        
        public void SetColorByColorType(ColorType type)
        {
            SetMats(Data.TeamData.GetColorsByType(type));
            GeneralEventbus.OnTowerColorChange?.Invoke(Id);
        }

        public void ToFreezeColor()
        {
            SetMats(Data.TeamData.ColorDatas.Select(c=>c.FreezeMaterial).ToArray());
        }

        public void ToBlueprintColor()
        {
            SetMats(Data.TeamData.ColorDatas.Select(c=>c.BlueprintMaterial).ToArray());
        }

        public void ToSelectionColor()
        {
            SetMats(Data.TeamData.ColorDatas.Select(c=>c.SelectedMaterial).ToArray());
            GeneralEventbus.OnTowerColorChange?.Invoke(Id);
        }

        public void ToOriginalColor()
        {
            SetMats(Data.TeamData.ColorDatas.Select(c=>c.DefaultMaterial).ToArray());
            GeneralEventbus.OnTurnTowerDeselect?.Invoke(Id);
        }
        
        
        // void SetTeamLogo()
        // {
        //     
        //     Data.Logo.transform.DOScale(Vector3.zero, Data.TimingData.colorFadeDuration / 2).
        //         OnComplete(() =>
        //     {
        //         Data.Logo.sprite = Data.TeamData.TeamLogo;
        //         Data.Logo.color = Data.TeamData.LogoMat.color;
        //        
        //         Data.Logo.transform.DOScale(Vector3.one, Data.TimingData.colorFadeDuration / 2);
        //     });
        // }
        
        // void FadeColor()
        // {
        //     colorChanger.FadeColors(Data.MiddleMeshes, Data.TeamData.TeamColor);
        // }
        
    }

}
