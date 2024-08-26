using System;
using System.Collections.Generic;
using Enums;
using Teams;
using Towers;

namespace _Core.Turn.Selectors
{
    public interface IBlockable
    {
        public void TryBlock(Dictionary<TeamState, Team> teamsByTurn);
    }
    public interface ITeamBlocker
    {
        public Selections.BlockType BlockType { get; set; }
        
        public TeamState BlockedTeamByTurn { get;}
        public void BlockSelection(Dictionary<TeamState, Team> teams);
    }

    // public class PlayerBlocker : ITeamBlocker
    // {
    //     public Selections.BlockState BlockState { get; set; }
    //     public TeamState BlockedTeamState { get; set; } = TeamState.CurrentTeam;
    //     public void BlockSelection(Dictionary<TeamState, Team> teams)
    //     {
    //         TeamData teamToBlock = teams[BlockedTeamState].Data;
    //         teamToBlock.Towers.ForEach(t=>t.DisableSelection());
    //     }
    // }
    //
    // public class RivalBlocker : ITeamBlocker
    // {
    //     public Selections.BlockState BlockState { get; set; }
    //     public TeamState BlockedTeamState { get; set; } = TeamState.RivalTeam;
    //     public void BlockSelection(Dictionary<TeamState, Team> teams)
    //     {
    //         TeamData teamToBlock = teams[BlockedTeamState].Data;
    //         teamToBlock.Towers.ForEach(t=>t.DisableSelection());
    //     }
    // }

    public class Blocker : ITeamBlocker // where TBlocker : ITeamBlocker
    {
        public Selections.BlockType BlockType { get; set; }
        public TeamState BlockedTeamByTurn
        {
            get
            {
                return BlockType switch
                {
                    Selections.BlockType.BlockCurrent => TeamState.CurrentTeam,
                    Selections.BlockType.BlockRival => TeamState.RivalTeam,
                    _ => throw new InvalidOperationException("Invalid BlockState value.")
                };
            }
        }

        public void BlockSelection(Dictionary<TeamState, Team> teams)
        {
            TeamData teamToBlock = teams[BlockedTeamByTurn].Data;
            teamToBlock.Towers.ForEach(t=>t.DisableSelection());
        }
        
        // public void Setup(Selections.BlockState blockState)
        // {
        //     BlockState = blockState;
        //     BlockedTeamState = BlockState switch
        //     {
        //         Selections.BlockState.BlockCurrent => TeamState.CurrentTeam,
        //         Selections.BlockState.BlockRival => TeamState.RivalTeam,
        //         _ => BlockedTeamState
        //     };
        // }
    }
}