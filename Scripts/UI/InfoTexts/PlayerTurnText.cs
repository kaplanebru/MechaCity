namespace UI
{
    public class PlayerTurnText : BaseInfoText
    {
        protected override void SubscribeEvents()
        {
            UIEventbus.OnTeamSwitch += UpdateInfoText;
        }


        protected override void SetInfoText(string teamName)
        {
            InfoText.text = "Turn: " + teamName;
        }

        protected override void Unsubscribe()
        {
            UIEventbus.OnTeamSwitch -= UpdateInfoText;
        }
    }
}