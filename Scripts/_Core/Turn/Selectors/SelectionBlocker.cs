using System.Collections.Generic;
using Enums;
using Teams;
using Towers;

namespace _Core.Turn.Selectors
{
    public class SelectionBlocker<TBlocker> where TBlocker : ITeamBlocker, new()
    {
        private TBlocker teamBlocker = new TBlocker();
        public void BlockSelection(Dictionary<TeamState, Team> teams)
        {
            TeamData teamToBlock = teams[teamBlocker.BlockedTeamState].Data;
            teamToBlock.Towers.ForEach(t=>AllTowers.GetTower(t.UniqID).DisableSelection());
        }

    }
}