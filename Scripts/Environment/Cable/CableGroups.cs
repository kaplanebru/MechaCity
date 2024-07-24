using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CableGroups : MonoBehaviour
{
    public Cable[] cables;
    public Color selectionColor;
    public Color defaultColor;

    private Cable _currentCable;

    private void OnEnable()
    {
        Eventbus.StateEvents.OnLinkStateBegin += DeselectAll;
        GeneralEventbus.OnTurnTowerSelection += ToSelection;
        GeneralEventbus.OnTurnTowerDeselect += Deselect;
    }

    private void Deselect(int id)
    {
        _currentCable = cables.FirstOrDefault(t => t.id == id);
        _currentCable.SetColor(defaultColor);
    }

    private void ToSelection(int id)
    {
        _currentCable = cables.FirstOrDefault(t => t.id == id); //_tubes[index]
        _currentCable.SetColor(selectionColor);
    }

    private void DeselectAll()
    {
        foreach (var cable in cables)
        {
            cable.SetColor(defaultColor);
        }
    }
    
    private void OnDisable()
    {
        Eventbus.StateEvents.OnLinkStateBegin -= DeselectAll;
        GeneralEventbus.OnTurnTowerSelection -= ToSelection;
        GeneralEventbus.OnTurnTowerDeselect -= Deselect;
    }
}