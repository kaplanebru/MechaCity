using System;
using System.Collections.Generic;
using Enums;

namespace Teams
{
    public static class TeamEvents
    {
        public static Action<Dictionary<TeamStatus, Team>> OnTeamsSent;
        public static Action OnBothTeamsRequest;
        public static Action<Team[]> OnTeamsSet;
        public static Func<TeamStatus, Team> OnTurnTeamDemand;
    }
}