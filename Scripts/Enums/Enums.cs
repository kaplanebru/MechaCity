namespace Enums
{
    public enum TeamType
    {
        Team1,
        Team2
    }

    public enum TeamState
    {
        CurrentTeam,
        RivalTeam
    }

    public enum TurnState
    {
        TurnStarted,
        TurnEnded
    }

    public enum TurnAction
    {
        Started,
        Completed
    }

    public enum TurnStateType
    {
        Selection,
        Link,
        Combat,
        Intruder
        //Exit
    }

    public enum GameEndState
    {
        GameStarted,
        Win,
        Lose
    }

    public enum BpType
    {
        Reverse,
        Freeze,
        Double
    }

    public enum SelectionType
    {
        PlayerOnly,
        RivalOnly,
        All
    }

    
    
}