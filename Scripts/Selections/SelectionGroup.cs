using System;
using System.Collections.Generic;
using Actor;
using Enums;
using Enums.Selections;
using Towers;
using UnityEngine;


[Serializable]
public class SelectionGroup
{
    public int Index;
    public int MaxTowers = 1;
    public ColorType SelectionColorType;
    public List<uint> SelectedActors { get; set; } = new();
    
    public BlockType BlockType;
    public TeamState SelectionTeam {
        get
        {
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
    public void ResetSelectedTowers()
    {
        // foreach (var actorID in SelectedActors)
        // {
        //    Debug.Log(actorID);
        // }
        if(SelectedActors.Count == 0) return;
        
        foreach (var actorID in SelectedActors)
        {
            ActorData actor = ActorHolder.Registry[actorID];
            foreach (var tower in actor.Towers)
            {
                tower.ColorHandler.ToOriginalColor();
            }
        }
        SelectedActors.Clear();
    }

    public void ResetAllTowers()
    {
        AllTowers.ResetTowerColors();
        SelectedActors.Clear();
    }

    public void ClearTowers()
    {
        SelectedActors.Clear();
    }
}