using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Teams;
using Enums;

namespace UI
{
    public abstract class BaseInfoText : MonoBehaviour
    {
        
        protected Dictionary<TeamType, string> TeamNamesByType = new();
        protected TextMeshProUGUI InfoText;

        private void OnEnable()
        {
            InfoText = GetComponentInChildren<TextMeshProUGUI>();
            SubscribeEvents();
        }

        protected virtual void SubscribeEvents()
        {
        }

        public void Setup(Dictionary<TeamType, string> teamNamesByType)
        {
            TeamNamesByType = teamNamesByType;
            SetInfoText(TeamNamesByType[0]);
        }

        public void UpdateInfoText(TeamType currentTeamType)
        {
            SetInfoText(
                TeamNamesByType[currentTeamType]); //(_teams.FirstOrDefault(t => t.Data.TeamType == currentTeamType).Data.Name);
        }

        protected abstract void SetInfoText(string teamName);

        protected virtual void Unsubscribe()
        {
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}