using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Clicks
{
    public class ClickHandler : MonoBehaviour
    {
        private Clickable[] _clickables;

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
    }

}
