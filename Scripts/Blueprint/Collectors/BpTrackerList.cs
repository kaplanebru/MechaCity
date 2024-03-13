using Enums;

namespace Blueprint
{
    public class BpTrackerList : BaseTrackerList
    {
        public override ITrackable CreateTracker(params object[] args)
        {
            return new BpLifeTracker(
                (int) args[0], 
                (int) args[1], 
                (BpType) args[2]);
        }
    }
}