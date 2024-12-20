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
        public void SetTeamsAndBlock(Dictionary<TeamState, Team> teamsByTurn);
    }

    public interface ITeamBlocker
    {
        public BlockType BlockType { get; set; }
        public void BlockSelection(Dictionary<TeamState, Team> teams, TeamState blockedTeam);
    }


    public class Blocker : ITeamBlocker // where TBlocker : ITeamBlocker
    {
        public BlockType BlockType { get; set; }
        

        public void BlockSelection(Dictionary<TeamState, Team> teams, TeamState blockedTeam)
        {
            if(BlockType == BlockType.None) return;
            TeamData teamToBlock = teams[blockedTeam].Data;
            teamToBlock.Actors.ForEach(a =>
            {
                foreach (var tower in a.Towers)
                {
                   tower.VisualData.DisableSelection();
                }
            });
        }
    }
}