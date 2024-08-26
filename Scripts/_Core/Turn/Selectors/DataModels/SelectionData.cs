using Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "Selection/" + nameof(SelectionData), fileName = nameof(SelectionData))]

public class SelectionData : ScriptableObject
{
    public SelectionType Type;
    public SelectionGroup[] Groups;
}