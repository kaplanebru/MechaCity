using Enums;
using UnityEngine;

[CreateAssetMenu(menuName = nameof(MultiSelection), fileName = nameof(MultiSelection))]
public class MultiSelection : ScriptableObject
{
    public SelectionType Type = SelectionType.All;
    public SelectionGroup[] Groups;
}