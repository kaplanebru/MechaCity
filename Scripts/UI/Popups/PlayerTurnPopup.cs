
using Enums;
using UnityEngine;

namespace GameUI
{
    public class PlayerTurnPopup : MonoBehaviour
    {
        public BaseInfoText popupText;
        private void OnEnable()
        {
            UIEventbus.OnActiveTeamSet += ShowPopup;
        }

        private void ShowPopup(TeamType currentTeamType)
        {
            popupText.gameObject.SetActive(true);
            popupText.UpdateInfoText(currentTeamType);
        }
    

        private void OnDisable()
        {
            UIEventbus.OnActiveTeamSet -= ShowPopup;
        }
    }

}
