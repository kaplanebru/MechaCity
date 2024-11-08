using System;
using Enums;
using UnityEngine;


namespace Clicks
{
    public class Clickable : BaseClickable<int> //uniqID
    {
        public uint id;
        public TeamType teamType;
        private Collider collider;

        private void Awake()
        {
            collider = GetComponent<Collider>();
            collider.enabled = false;
            Invoke(nameof(EnableSelection), 3); //todo
        }

        void EnableSelection()
        {
            collider.enabled = true;
        }

        void OnMouseEnter()
        {
            ShowTowerInfo();
        }

        void OnMouseExit()
        {
            HideTowerInfo();
        }

        void ShowTowerInfo()
        {
            Eventbus.IndicatorEvents.OnActorHover?.Invoke(id);
            Debug.Log("Hovering over: " + gameObject.name);
        }

        void HideTowerInfo()
        {
            // Hide tower information
        }

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
