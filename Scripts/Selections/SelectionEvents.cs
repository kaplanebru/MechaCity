using System;

public static class SelectionEvents
{
    public static Action<Selector> OnSelectionReady;
    public static Action<string> OnSelection;
}