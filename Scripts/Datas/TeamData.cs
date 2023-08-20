using UnityEngine;

namespace Datas
{
    [CreateAssetMenu(fileName = nameof(TeamData))]
    public class TeamData : ScriptableObject
    {
        public Team Team;
        public int Id;
        public Material DefaultMaterial;
        public Material SelectedMaterial;
    }
}
