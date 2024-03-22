using Enums;

namespace _Core.Turn.Selectors
{
    public interface ITeamBlocker
    {
        public TeamState BlockedTeamState { get; set; }
    }

    public class PlayerBlocker : ITeamBlocker
    {
        public TeamState BlockedTeamState { get; set; } = TeamState.CurrentTeam;
    }

    public class RivalBlocker : ITeamBlocker
    {
        public TeamState BlockedTeamState { get; set; } = TeamState.RivalTeam;
    }
}