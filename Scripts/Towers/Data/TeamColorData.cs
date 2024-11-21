using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Towers
{
    [Serializable]
    public  class ColorData
    {
        public ColorDistrict DistrictType;
        public Material DefaultMaterial;
        public Material SelectedMaterial;
        public Material BlueprintMaterial;
        public Material FreezeMaterial;
    }
    
    [CreateAssetMenu(fileName = nameof(TeamColorData))]
    public class TeamColorData : ScriptableObject
    {
        public TeamType TeamType;

        public ColorData[] ColorDatas;
        

        // public Sprite TeamLogo;
        // public Material LogoMat;
        public Color TeamColor { get; set; }

        private Dictionary<ColorType, Material[]> ColorsByType = new();
        public Material[] GetColorsByType(ColorType type) => ColorsByType[type];

        private void OnEnable() //todo: fix
        {
            SetTeamColors();
        }

        private void SetTeamColors()
        {
            TeamColor = ColorDatas[0].DefaultMaterial.color;
            SetColorsByType();
        }

        private void SetColorsByType()
        {
            ColorsByType.Add(ColorType.Default, ColorDatas.Select(c=>c.DefaultMaterial).ToArray()); 
            ColorsByType.Add(ColorType.Selection, ColorDatas.Select(c=>c.SelectedMaterial).ToArray());
            ColorsByType.Add(ColorType.Blueprint,  ColorDatas.Select(c=>c.BlueprintMaterial).ToArray());
            ColorsByType.Add(ColorType.Freeze, ColorDatas.Select(c=>c.FreezeMaterial).ToArray());
        }
    }
}