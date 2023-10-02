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
            // clickableParent = GetComponentInParent<Tower>();
            // Eventbus.TeamEvents.OnTowerTeamSet += SetTeam;
        }

        // private void SetTeam(TeamType _teamType, Tower tower)
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
