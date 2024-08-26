using Enums;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(SelectionData), fileName = nameof(SelectionData))]

public class SelectionData : ScriptableObject
{
    public SelectionType Type;
    public SelectionGroup[] Groups;
}