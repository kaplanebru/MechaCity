namespace GameUI
{
    public class PlayerTurnText : BaseInfoText
    {
        protected override void SubscribeEvents()
        {
            UIEventbus.OnActiveTeamSet += UpdateInfoText;
        }


        public override void SetInfoText(string teamName)
        {
            InfoText.text = "Turn: " + teamName;
        }

        protected override void Unsubscribe()
        {
            UIEventbus.OnActiveTeamSet -= UpdateInfoText;
        }
    }
}