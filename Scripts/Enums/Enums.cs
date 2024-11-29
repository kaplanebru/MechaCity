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

    namespace Selections
    {
        public enum SelectionType
        {
            PlayerOnlyStd,
            PlayerOnlyBp,
            RivalOnlyBp,
            SinglePlayerOnlyBP,
            SingleRivalOnlyBP,
            All, 
            None
        }
        public enum BlockType
        {
            BlockCurrent,
            BlockRival,
            None
        }

        public enum ColorType
        {
            Default,
            Selection,
            Blueprint,
            Freeze
        }

        public enum ColorDistrict
        {
            OuterShell,
            InnerShell,
            Inside
        }
    }

    public enum ActorType
    {
        Standard,
        MultiTower,
    }

    public enum ActorUnit
    {
        Relation,
        Health,
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
        DoubleSelf,
        Shield,
        BreakShield,
        None
    }

    public enum PersonaType
    {
        Jester,
        Fighter,
        Defender,
        Common
    }

    public enum IndicatorState
    {
        Friendly,
        Enemy
    }

   

   

    
    
}