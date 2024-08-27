using System;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using UnityEngine;

namespace Towers
{
    [CreateAssetMenu(fileName = nameof(TeamTowerData))]
    public class TeamTowerData : ScriptableObject
    {
        public TeamType TeamType;
        public Material[] DefaultMaterial;
        public Material[] SelectedMaterial;
        public Material[] BlueprintMaterial;
        public Material[] FreezeMaterial;
        
        public Material[] DeadMaterial;
        public Material[] RegenerationMaterial;

        public Sprite TeamLogo;
        public Material LogoMat;
        public Color[] TeamColors { get; set; }

        private Dictionary<ColorType, Material[]> ColorByType = new ();
        public Material[] GetColorByType(ColorType type) => ColorByType[type];
        private void OnEnable() //todo: fix
        {
            SetTeamColors();
        }

        private void SetTeamColors()
        {
            TeamColors = new Color[DefaultMaterial.Length];
            for (var i = 0; i < DefaultMaterial.Length; i++)
            {
                TeamColors[i] = DefaultMaterial[i].color;
            }
            
            SetColorsByType();
           // Debug.Log(ColorByType.Count + " " + ColorByType[Selections.ColorType.Selection][0].name);
        }

        private void SetColorsByType()
        {
            ColorByType.Add(ColorType.Default, DefaultMaterial);
            ColorByType.Add(ColorType.Selection, SelectedMaterial);
            ColorByType.Add(ColorType.Blueprint, BlueprintMaterial);
            ColorByType.Add(ColorType.Freeze, FreezeMaterial);
        }
    }
 
}
