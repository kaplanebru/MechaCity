using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class CableGroups
    {
        Color _selectionColor;
        Color _defaultColor;
        
        private Cable[] _cables;
        private Cable _currentCable;

        public CableGroups(Cable[] cables)
        {
            _cables = cables;
        }

        public void Subscribe()
        {
            // Eventbus.LinkEvents.OnLinkStateBegin += DeselectAll;
            // GeneralEventbus.OnTowerColorChange += ToSelection;
            // GeneralEventbus.OnTurnTowerDeselect += Deselect;
        }

        public void SetColor(Color selection, Color defaultColor)
        {
            _defaultColor = defaultColor;
            _selectionColor = selection;
        }

        private void Deselect(int id)
        {
            _currentCable = _cables.FirstOrDefault(t => t.id == id);
            _currentCable.SetColor(_defaultColor);
        }

        private void ToSelection(int id)
        {
            _currentCable = _cables.FirstOrDefault(t => t.id == id); //_tubes[index]
            _currentCable.SetColor(_selectionColor);
        }

        private void DeselectAll()
        {
            foreach (var cable in _cables)
            {
                cable.SetColor(_defaultColor);
            }
        }

        public void Unsubscribe()
        {
            //Eventbus.LinkEvents.OnLinkStateBegin -= DeselectAll;
            GeneralEventbus.OnTowerColorChange -= ToSelection;
            GeneralEventbus.OnTurnTowerDeselect -= Deselect;
        }
    }
}