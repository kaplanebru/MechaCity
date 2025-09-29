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
    public ColorState selectionColorState;
    public List<uint> SelectedActors { get; set; } = new();
    
    public BlockType BlockType;
    public TeamStatus SelectionTeam {
        get
        {
            return BlockType switch
            {
                BlockType.BlockCurrent => TeamStatus.PassiveTeam,
                BlockType.BlockRival => TeamStatus.ActiveTeam,
                _ => throw new InvalidOperationException("BlockType None")
            };
        }
    }
    
    public TeamStatus BlockedTeam
    {
        get
        {
            return BlockType switch
            {
                BlockType.BlockCurrent => TeamStatus.ActiveTeam,
                BlockType.BlockRival => TeamStatus.PassiveTeam,
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
            ActorData actor = ActorDB.Registry[actorID];
            foreach (var tower in actor.Towers)
            {
                tower.VisualData.ColorHandler.ToOriginalSelectionColor();
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