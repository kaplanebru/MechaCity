using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = nameof(TeamTowerData))]
    public class TeamTowerData : ScriptableObject
    {
        public TeamType TeamType;
        public Material DefaultMaterial;
        public Material SelectedMaterial;
        public Material DeadMaterial;
    }
}
