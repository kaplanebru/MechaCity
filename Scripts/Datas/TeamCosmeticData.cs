using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = nameof(TeamCosmeticData))]
    public class TeamCosmeticData : ScriptableObject
    {
        public TeamType teamType;
        public Material DefaultMaterial;
        public Material SelectedMaterial;
        public Material DeadMaterial;
    }
}
