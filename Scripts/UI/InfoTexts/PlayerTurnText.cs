using Network;
using UnityEngine;

namespace GameUI
{
    public class PlayerTurnText : BaseInfoText
    {
        protected override void SubscribeEvents()
        {
            NetworkEventbus.UIEvents.OnActiveTeamSet += UpdateInfoText;
        }


        public override void SetInfoText(string teamName)
        {
            int turnTrack = TurnTracker.GetTurnTracker() == 0 ? 1 : TurnTracker.GetTurnTracker();
            infoText.text = null;
            infoText.text = "Turn " + turnTrack + ": " + teamName;
            
            Debug.Log("Updating text to: " + "Turn " + turnTrack + ": " + teamName);
        }

        protected override void Unsubscribe()
        {
            NetworkEventbus.UIEvents.OnActiveTeamSet -= UpdateInfoText;
        }
    }
}