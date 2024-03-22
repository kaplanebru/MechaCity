using System.Collections.Generic;
using Enums;
using Teams;
using Towers;

namespace _Core.Turn.Selectors
{
    public interface ITeamBlocker
    {
        public TeamState BlockedTeamState { get; set; }
        public void BlockSelection(Dictionary<TeamState, Team> teams);
    }

    public class PlayerBlocker : ITeamBlocker
    {
        public TeamState BlockedTeamState { get; set; } = TeamState.CurrentTeam;
        public void BlockSelection(Dictionary<TeamState, Team> teams)
        {
            TeamData teamToBlock = teams[BlockedTeamState].Data;
            teamToBlock.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).DisableSelection());
        }
    }

    public class RivalBlocker : ITeamBlocker
    {
        public TeamState BlockedTeamState { get; set; } = TeamState.RivalTeam;
        public void BlockSelection(Dictionary<TeamState, Team> teams)
        {
            TeamData teamToBlock = teams[BlockedTeamState].Data;
            teamToBlock.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).DisableSelection());
        }
    }
}