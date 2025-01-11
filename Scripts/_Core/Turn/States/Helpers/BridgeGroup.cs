using Towers;

public class BridgeGroup
{
    public int First;
    public int Second;

    private TowerNumericData firstTower;
    private TowerNumericData secondTower;

    public BridgeGroup(int first, int second)
    {
        First = first;
        Second = second;
        GetTowers();
        ReorderByHeight();
    }

    void GetTowers()
    {
        firstTower = AllTowers.GetNumericData(First);
        secondTower = AllTowers.GetNumericData(Second);
    }

    void ReorderByHeight()
    {
        if (firstTower.Height > secondTower.Height)
            (First, Second) = (Second, First);
    }
}