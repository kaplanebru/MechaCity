namespace UI
{
    public class PopupTurnText : BaseInfoText
    {
        protected override void SetInfoText(string teamName)
        {
            InfoText.text = teamName + "'s Turn"; 
        }
    }

}
