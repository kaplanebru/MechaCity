using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Enums;

namespace GameUI
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
            SetInfoText(TeamNamesByType[currentTeamType]);
        }

        public abstract void SetInfoText(string teamName);

        protected virtual void Unsubscribe()
        {
        }

        private void OnDisable()
        {
            Unsubscribe();
        }
    }
}