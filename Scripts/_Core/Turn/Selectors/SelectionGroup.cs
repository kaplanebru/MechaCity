using System;
using System.Collections.Generic;
using Enums;
using Towers;


[Serializable]
public class SelectionGroup
{
    public int Index;
    public int MaxTowers = 1;
    public Selections.BlockType BlockType;
    public Selections.ColorType SelectionColorType;
    public List<int> SelectedTowers { get; set; } = new();
    public void ResetTowers()
    {
        foreach (var tower in SelectedTowers)
        {
            AllTowers.GetData(tower).ColorHandler.ToOriginalColor();
        }
        SelectedTowers.Clear();
    }
    
    // public TeamState BlockedTeamState
    // {
    //     get
    //     {
    //         return blockType switch
    //         {
    //             Selections.BlockType.BlockCurrent => TeamState.CurrentTeam,
    //             Selections.BlockType.BlockRival => TeamState.RivalTeam,
    //             _ => throw new InvalidOperationException("Invalid BlockState value.")
    //         };
    //     }
    // }
    
}