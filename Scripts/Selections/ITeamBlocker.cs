using System;
using System.Collections.Generic;
using Enums;
using Enums.Selections;
using Teams;
using UnityEngine;

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
                Debug.Log(BlockType);

                return BlockType switch
                {
                    BlockType.BlockCurrent => TeamState.CurrentTeam,
                    BlockType.BlockRival => TeamState.RivalTeam,
                    _ => throw new InvalidOperationException("BlockType None")
                };
            }
        }

        public TeamState SelectionTeamByTurn
        {
            get
            {
                Debug.Log(BlockType);
                switch (BlockType)
                {
                    case BlockType.BlockCurrent:
                        return TeamState.RivalTeam;
                    case BlockType.BlockRival:
                        return TeamState.CurrentTeam;
                    default:
                        throw new InvalidOperationException("BlockType None");
                }
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