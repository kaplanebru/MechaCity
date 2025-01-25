
using Enums;
using Network;
using UnityEngine;

namespace GameUI
{
    public class PlayerTurnPopup : MonoBehaviour
    {
        public BaseInfoText popupText;
        private void OnEnable()
        {
            NetworkEventbus.UIEvents.OnActiveTeamSet += ShowPopup;
        }

        private void ShowPopup(TeamType currentTeamType)
        {
            popupText.gameObject.SetActive(true);
            popupText.UpdateInfoText(currentTeamType);
        }
    

        private void OnDisable()
        {
            NetworkEventbus.UIEvents.OnActiveTeamSet -= ShowPopup;
        }
    }

}
