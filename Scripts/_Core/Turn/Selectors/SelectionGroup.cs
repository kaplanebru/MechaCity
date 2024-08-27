using System;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using Towers;


[Serializable]
public class SelectionGroup
{
    public int Index;
    public int MaxTowers = 1;
    public BlockType BlockType;
    public ColorType SelectionColorType;
    public List<int> SelectedTowers { get; set; } = new();
    public void ResetTowers()
    {
        foreach (var tower in SelectedTowers)
        {
            AllTowers.GetData(tower).ColorHandler.ToOriginalColor();
        }
        SelectedTowers.Clear();
    }
}