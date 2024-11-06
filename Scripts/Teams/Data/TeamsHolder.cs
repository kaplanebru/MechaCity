using UnityEngine;

namespace Teams
{
    [CreateAssetMenu(fileName = nameof(TeamsHolder))]
    public class TeamsHolder : ScriptableObject
    {
        public TeamData[] TeamsData;
    }
}
