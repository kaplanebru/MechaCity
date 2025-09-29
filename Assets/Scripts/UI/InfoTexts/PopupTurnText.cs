namespace GameUI
{
    public class PopupTurnText : BaseInfoText
    {
        public override void SetInfoText(string teamName)
        {
            infoText.text = teamName + "'s Turn"; 
        }
    }

}
