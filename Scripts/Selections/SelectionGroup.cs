using System;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using Towers;
using UnityEngine;


[Serializable]
public class SelectionGroup
{
    public int Index;
    public int MaxTowers = 1;
    public BlockType BlockType;
    public TeamState SelectionTeam {
        get
        {
            Debug.Log(BlockType);

            return BlockType switch
            {
                BlockType.BlockCurrent => TeamState.RivalTeam,
                BlockType.BlockRival => TeamState.CurrentTeam,
                _ => throw new InvalidOperationException("BlockType None")
            };
        }
    }
    
    public TeamState BlockedTeam
    {
        get
        {
            return BlockType switch
            {
                BlockType.BlockCurrent => TeamState.CurrentTeam,
                BlockType.BlockRival => TeamState.RivalTeam,
                _ => throw new InvalidOperationException("BlockType None")
            };
        }
    }
    
    
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