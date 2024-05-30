namespace GameUI
{
    public class PopupTurnText : BaseInfoText
    {
        public override void SetInfoText(string teamName)
        {
            InfoText.text = teamName + "'s Turn"; 
        }
    }

}
