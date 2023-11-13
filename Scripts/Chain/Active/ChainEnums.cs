namespace Chain
{
    public class ChainEnums 
    {
        public enum ChainType
        {
            BikeChain,
            StandardChain,
            Line
        }
    
        public enum ChainDirection
        {
            Clockwise,
            ReverseClock
        }

        public enum HoleType
        {
            Basic,
            Complex,
            Custom,
            None
        }

        public enum CogContactType
        {
            ChainRelated,
            CogRelated,
            Indifferent
        }

    }

}
