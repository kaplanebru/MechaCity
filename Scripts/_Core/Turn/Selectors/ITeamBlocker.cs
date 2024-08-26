using System.Collections.Generic;
using Enums;
using Teams;
using Towers;

namespace _Core.Turn.Selectors
{
    public interface IBlockable
    {
        public void TryBlock(Dictionary<TeamState, Team> teams);
    }
    public interface ITeamBlocker
    {
        public Selections.BlockState BlockState { get; set; }
        public TeamState BlockedTeamState { get; set; }
        public void BlockSelection(Dictionary<TeamState, Team> teams);
    }

    public class PlayerBlocker : ITeamBlocker
    {
        public Selections.BlockState BlockState { get; set; }
        public TeamState BlockedTeamState { get; set; } = TeamState.CurrentTeam;
        public void BlockSelection(Dictionary<TeamState, Team> teams)
        {
            TeamData teamToBlock = teams[BlockedTeamState].Data;
            teamToBlock.Towers.ForEach(t=>t.DisableSelection());
        }
    }

    public class RivalBlocker : ITeamBlocker
    {
        public Selections.BlockState BlockState { get; set; }
        public TeamState BlockedTeamState { get; set; } = TeamState.RivalTeam;
        public void BlockSelection(Dictionary<TeamState, Team> teams)
        {
            TeamData teamToBlock = teams[BlockedTeamState].Data;
            teamToBlock.Towers.ForEach(t=>t.DisableSelection());
        }
    }

    public class Blocker<TBlocker> where TBlocker : ITeamBlocker
    {
        public Selections.BlockState BlockState { get; set; }
        private TeamState BlockedTeamState { get; set; }

        public void Setup(Selections.BlockState blockState)
        {
            BlockState = blockState;
            BlockedTeamState = BlockState switch
            {
                Selections.BlockState.BlockCurrent => TeamState.CurrentTeam,
                Selections.BlockState.BlockRival => TeamState.RivalTeam,
                _ => BlockedTeamState
            };
        }
        
        public void BlockSelection(Dictionary<TeamState, Team> teams)
        {
            TeamData teamToBlock = teams[BlockedTeamState].Data;
            teamToBlock.Towers.ForEach(t=>t.DisableSelection());
        }
    }
}