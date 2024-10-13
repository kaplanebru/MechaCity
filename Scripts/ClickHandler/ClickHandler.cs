using System;
using Enums;
using UnityEngine;

namespace Clicks
{
    public class ClickHandler : MonoBehaviour //, ITowerRelated
    {
        private Clickable[] _clickables;

        private void OnEnable()
        {
            Initialize();
            //todo: event for setclickableıds
        }

        
        public void Initialize()
        {
            _clickables = GetComponentsInChildren<Clickable>();
        }
        
        void SetClickableIds(uint id)
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

        private void OnDisable()
        {
            
        }
    }
}