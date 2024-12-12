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
        public SelectionMeshes SelectionMeshes;
        public CombatTimingData TimingData;
    }

    [Serializable]
    public class SelectionMeshes
    {
        public MeshRenderer Light;
        public MeshRenderer[] Head;
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
            SetTeamMats(teamData.GetColorsByType(ColorDistrict.InnerShell, ColorState.Default));
            SetTeamMats(teamData.GetColorsByType(ColorDistrict.OuterShell, ColorState.Default));
            SetSelectionMats(teamData.GetColorsByType(ColorDistrict.Inside, ColorState.Default));
        }
        
        private void SetCombinedMeshes()
        {
            foreach (var meshCombiner in Data.MeshCombiners)
            {
                meshCombiner.CombineMeshes(out MeshRenderer renderer);
                combinedRenderers.Add(renderer);
            }
        }

        private void SetTeamMats(params Material[] mats)
        {
            colorChanger.SetMaterial(mats[0], Data.MiddleMeshes);
            colorChanger.SetMaterial(mats[1],combinedRenderers.ToArray());
        }

        private void SetSelectionMats(params Material[] mats)
        {
            colorChanger.SetMaterial(mats[0], Data.SelectionMeshes.Light);
            colorChanger.SetMaterial(mats[1], Data.SelectionMeshes.Head);
        }
        
        public void SetColorByColorType(ColorState state)
        {
            SetSelectionMats(Data.TeamData.GetColorsByType(ColorDistrict.Inside, state));
            GeneralEventbus.OnTowerColorChange?.Invoke(Id);
        }

        public void ToFreezeColor()
        {
            SetSelectionMats(Data.TeamData.GetColorsByType(ColorDistrict.Inside, ColorState.Freeze));
        }

        public void ToBlueprintColor()
        {
            SetSelectionMats(Data.TeamData.GetColorsByType(ColorDistrict.Inside, ColorState.Blueprint));
        }

        public void ToSelectionColor()
        {
            SetSelectionMats(Data.TeamData.GetColorsByType(ColorDistrict.Inside, ColorState.Selection));
            GeneralEventbus.OnTowerColorChange?.Invoke(Id);
        }

        public void ToOriginalColor()
        {
            SetSelectionMats(Data.TeamData.GetColorsByType(ColorDistrict.Inside, ColorState.Default));
            GeneralEventbus.OnTurnTowerDeselect?.Invoke(Id);
        }
    }

}
