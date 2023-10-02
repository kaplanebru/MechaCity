namespace UI
{
    public class PlayerTurnText : BaseInfoText
    {
        protected override void SubscribeEvents()
        {
            Eventbus.UIEvents.OnTeamSwitch += UpdateInfoText;
        }


        protected override void SetInfoText(string teamName)
        {
            infoText.text = "Turn: " + teamName;
        }

        protected override void Unsubscribe()
        {
            Eventbus.UIEvents.OnTeamSwitch -= UpdateInfoText;
        }
    }
}