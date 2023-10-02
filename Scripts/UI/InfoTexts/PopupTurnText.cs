namespace UI
{
    public class PopupTurnText : BaseInfoText
    {
        protected override void SetInfoText(string teamName)
        {
            infoText.text = teamName + "'s Turn"; 
        }
    }

}
