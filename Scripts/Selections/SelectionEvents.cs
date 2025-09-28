using System;

public static class SelectionEvents
{
    public static Action<Selector> OnSelectionReady;
    public static Action OnSelectionTerminated;
    public static Action<uint> OnSelection;
    public static Action<uint> OnDeselect;
    public static Action OnDeselectAll;

   

}