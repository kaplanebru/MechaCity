using System;
using System.Collections.Generic;
using Enums;

namespace Teams
{
    public static class TeamEvents
    {
        public static Action<Dictionary<TeamState, Team>> OnTeamsSent;
        public static Action OnBothTeamsRequest;
        public static Action<Team[]> OnTeamsSet;
        public static Func<TeamState, Team> OnSingleTeamDemand;
    }
}