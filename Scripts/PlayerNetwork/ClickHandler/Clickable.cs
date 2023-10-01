using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Towers;


namespace ClickHandler
{
    public class Clickable : BaseClickable<Tower>
    {
        public int id; //for multiplayer
        public TeamType teamType;
   

        protected override void Setup()
        {
            clickableObject = GetComponentInParent<Tower>();
            Eventbus.TeamEvents.OnTowerTeamSet += SetTeam;
        }

        private void SetTeam(TeamType _teamType, Tower tower)
        {
            if(clickableObject !=  tower) return; //diğer teamdekiler etkilenir
            teamType = _teamType;
        }
    
        public override void UnsubscribeFromEvent()
        {
            Eventbus.TeamEvents.OnTowerTeamSet -= SetTeam;
        }
    }

}
