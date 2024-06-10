using System;
using Enums;
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

        private void OnEnable()
        {
            SetTeamColors();
        }

        void SetTeamColors()
        {
            TeamColors = new Color[DefaultMaterial.Length];
            for (var i = 0; i < DefaultMaterial.Length; i++)
            {
                TeamColors[i] = DefaultMaterial[i].color;
            }
        }
    }
}
