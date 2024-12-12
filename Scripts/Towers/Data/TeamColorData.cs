using System;
using System.Collections.Generic;
using System.Linq;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Towers
{

    [Serializable]
    public class ColorData
    {
        public ColorDistrict DistrictType;
        public Material[] DefaultMaterials;
        public Material[] SelectedMaterials;
        public Material[] BlueprintMaterials;
        public Material[] FreezeMaterials;
    }

    public class DistrictColors
    {
        public ColorDistrict District;
        public Dictionary<ColorState, Material[]> ColorsByState = new();
    }
    
    [CreateAssetMenu(fileName = nameof(TeamColorData))]
    public class TeamColorData : ScriptableObject
    {
        public TeamType TeamType;
        public ColorData[] ColorDatas;
        private Dictionary<ColorDistrict, DistrictColors> DistrictColors = new();
        

     
        public Color TeamColor { get; set; }

        private Dictionary<ColorState, Material[]> ColorsByColorType = new();

        public Material[] GetColorsByType(ColorDistrict districtType, ColorState state)
        {
           return DistrictColors[districtType].ColorsByState[state];
        } 

        private void OnEnable() //todo: fix
        {
            SetTeamColors();
        }

        private void SetTeamColors()
        {
            SetDistrictColors();
            TeamColor = DistrictColors[ColorDistrict.OuterShell].ColorsByState[ColorState.Default][0].color;
        }

        private void SetDistrictColors()
        {
            foreach (ColorDistrict colorDistrict in Enum.GetValues(typeof(ColorDistrict)))
            {
                DistrictColors.Add(colorDistrict, new DistrictColors());
                DistrictColors[colorDistrict].District = colorDistrict;
                
                var colorData = ColorDatas.FirstOrDefault(cd => cd.DistrictType == colorDistrict);
                var colorsByState = DistrictColors[colorDistrict].ColorsByState;
                
                colorsByState.Add(ColorState.Default, colorData.DefaultMaterials);
                colorsByState.Add(ColorState.Selection, colorData.SelectedMaterials);
                colorsByState.Add(ColorState.Blueprint, colorData.BlueprintMaterials);
                colorsByState.Add(ColorState.Freeze, colorData.FreezeMaterials);
            }
        }
        
    }
}