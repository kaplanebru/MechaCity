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
        Exit,
        Combat,
        Intruder
     
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
        Double,
        SelectionIncrement,
        None
    }

    public enum SelectionType
    {
        PlayerOnly,
        RivalOnly,
        All, 
        None
    }

    
    
}