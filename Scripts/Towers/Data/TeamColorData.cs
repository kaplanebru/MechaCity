using System;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = nameof(TeamColorData))]
    public class TeamColorData : ScriptableObject
    {
        public TeamType TeamType;

        [Header("Outer Shell")] 
        public Material DefaultMaterial;
        public Material SelectedMaterial;
        public Material BlueprintMaterial;
        public Material FreezeMaterial;

        [Header("Inner Shell")] 
        public Material CombinedMat;
        public Material SelectedCombinedMat;
        public Material BpCombinedMat;
        public Material FreezeCombinedMat;

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
            TeamColor = DefaultMaterial.color;
            SetColorsByType();
        }

        private void SetColorsByType()
        {
            ColorsByType.Add(ColorType.Default, new [] {DefaultMaterial, CombinedMat});
            ColorsByType.Add(ColorType.Selection, new [] {SelectedMaterial, SelectedCombinedMat});
            ColorsByType.Add(ColorType.Blueprint,  new [] {BlueprintMaterial, BpCombinedMat});
            ColorsByType.Add(ColorType.Freeze,  new [] {FreezeMaterial, FreezeCombinedMat});
        }
    }
}