
using Enums;
using UnityEngine;

namespace UI
{
    public class PlayerTurnPopup : MonoBehaviour
    {
        public BaseInfoText popupText;
        private void OnEnable()
        {
            Eventbus.UIEvents.OnTeamSwitch += ShowPopup;
        }

        private void ShowPopup(TeamType currentTeamType)
        {
            popupText.gameObject.SetActive(true);
            popupText.UpdateInfoText(currentTeamType);
        }
    

        private void OnDisable()
        {
            Eventbus.UIEvents.OnTeamSwitch -= ShowPopup;
        }
    }

}
