using System.Collections.Generic;

public interface ITrackable
{
    public int Lifespan { get; set; }
    public int RelatedTower { get; set; }

    public void ReduceValue();
}

public abstract class BaseTrackerList
{
    private  List<ITrackable> Collector = new();

    public virtual void Subscribe()
    {
        BpEventbus.LifespanEvents.OnTrackerRequest += AddToTrackList;
    }
    
    public void ReduceValueForAll()
    {
        for (var i = Collector.Count - 1; i >= 0; i--)
        {
            var tracker = Collector[i];
            tracker.ReduceValue();
        }
    }
    public abstract ITrackable CreateTracker(params object[] args);
    
    public void AddToTrackList(ITrackable tracker)
    {
        Collector.Add(tracker);
    }

    public void RemoveFromTrackList(ITrackable tracker)
    {
        Collector.Remove(tracker);
    }

    public virtual void Unsubscribe()
    {
        BpEventbus.LifespanEvents.OnTrackerRequest -= AddToTrackList;
    }
}

