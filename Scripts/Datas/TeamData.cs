using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = nameof(TeamData))]
    public class TeamData : ScriptableObject
    {
        public Enums.Team Team;
        public Material DefaultMaterial;
        public Material SelectedMaterial;
    }
}
