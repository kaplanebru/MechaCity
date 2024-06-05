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
    }
}
