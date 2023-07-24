using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = nameof(TeamData))]
    public class TeamData : ScriptableObject
    {
        public Team Team;
        public Material DefaultMaterial;
        public Material SelectedMaterial;
    }
}
