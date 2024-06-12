using Enums;
using UnityEngine;

namespace Clicks
{
    public class ClickHandler : MonoBehaviour, ITowerRelated
    {
        private Clickable[] _clickables;
        
        public int Id { get; set; }
        public void Initialize(int id)
        {
            _clickables = GetComponentsInChildren<Clickable>();
            Id = id;
            foreach (var clickable in _clickables)
            {
                clickable.id = Id;
            }
        }

        public void SetClickables(int id)
        {
            _clickables = GetComponentsInChildren<Clickable>();
            SetClickableIds(id);
        }

        void SetClickableIds(int id)
        {
            foreach (var clickable in _clickables)
            {
                clickable.id = id;
            }
        }

        public void SetClickableTeams(TeamType teamType)
        {
            foreach (var clickable in _clickables)
            {
                clickable.teamType = teamType;
            }
        }

        public void DisableSelection()
        {
            foreach (var clickable in _clickables)
            {
                clickable.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }

        public void EnableSelection()
        {
            foreach (var clickable in _clickables)
            {
                clickable.gameObject.layer = LayerMask.NameToLayer("Clickable");
            }
        }

       
    }
}