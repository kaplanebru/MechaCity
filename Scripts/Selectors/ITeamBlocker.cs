using System;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using Teams;

namespace _Core.Turn.Selectors
{
    public interface IBlockable
    {
        public void TryBlock(Dictionary<TeamState, Team> teamsByTurn);
    }

    public interface ITeamBlocker
    {
        public BlockType BlockType { get; set; }

        public TeamState BlockedTeamByTurn { get; }
        public void BlockSelection(Dictionary<TeamState, Team> teams);
    }


    public class Blocker : ITeamBlocker // where TBlocker : ITeamBlocker
    {
        public BlockType BlockType { get; set; }

        public TeamState BlockedTeamByTurn
        {
            get
            {
                return BlockType switch
                {
                    BlockType.BlockCurrent => TeamState.CurrentTeam,
                    BlockType.BlockRival => TeamState.RivalTeam,
                    _ => throw new InvalidOperationException("Invalid BlockState value.")
                };
            }
        }

        public void BlockSelection(Dictionary<TeamState, Team> teams)
        {
            if(BlockType == BlockType.None) return;
            TeamData teamToBlock = teams[BlockedTeamByTurn].Data;
            teamToBlock.Towers.ForEach(t => t.DisableSelection());
        }
    }
}