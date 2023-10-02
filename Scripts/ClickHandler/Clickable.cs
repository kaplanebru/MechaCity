using Enums;



namespace Clicks
{
    public class Clickable : BaseClickable<int> //uniqID
    {
        public int id;
        public TeamType teamType;
   

        protected override void Setup()
        {
            // clickableParent = GetComponentInParent<Tower>();
            // Eventbus.TeamEvents.OnTowerTeamSet += SetTeam;
        }

        // private void SetTeam(TeamType _teamType, Tower tower) //id de karşılaştırılabilir
        // {
        //     if(clickableParent !=  tower) return; //diğer teamdekiler etkilenir
        //     teamType = _teamType;
        // }
    
        public override void UnsubscribeFromEvent()
        {
            //Eventbus.TeamEvents.OnTowerTeamSet -= SetTeam;
        }
    }

}
