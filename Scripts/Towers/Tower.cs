using System;
using System.Collections;
using Blueprint;
using Clicks;
using DataModels;
using DG.Tweening;
using GameUI;
using UnityEngine;


namespace Towers
{
    public class Tower : MonoBehaviour
    {
        public TowerConstantData ConstantData;
        public TowerData Data;
        public CombatTimingData timingData;
        
        public TowerParts towerParts;
        public ClickHandler clickHandler;

        public TowerColorHandler ColorHandler;
        


        private void OnEnable()
        {
            towerParts = GetComponent<TowerParts>();
            clickHandler = GetComponent<ClickHandler>();
            ColorHandler = new TowerColorHandler(Data, towerParts);
            
            GeneralEventbus.OnTeamChange += FadeColor;
        }

        public void Setup(TeamTowerData teamTowerData)
        {
            Data.Height = ConstantData.StartHeight;
            Data.Health = ConstantData.StartHealth;
            Data.DamagePower = ConstantData.DamagePower;
            RestoreBullets();
            

            UIEventbus.OnHealthChange.Invoke(Data.Health, gameObject);
            clickHandler.SetClickables(Data.UniqID);
            Data.BpTowerData = new BpTowerData(Data.UniqID);

            towerParts.Initialize();
            SetTeam(teamTowerData);
        }

        public void SetTeam(TeamTowerData teamTowerData)
        {
            
            Data.TeamTowerData = teamTowerData;
            clickHandler.SetClickableTeams(teamTowerData.TeamType);
            
            GeneralEventbus.OnTeamChange.Invoke(Data.UniqID);  //x36 oluyor
        }

        void FadeColor(int id)
        {
            if(id != Data.UniqID) return;
            towerParts.FadeColor(Data.TeamTowerData);
        }

        public void EnableSelection()
        {
            if(!Data.IsClickable) return;
            clickHandler.EnableSelection();
        }

        public void DisableSelection()
        {
            clickHandler.DisableSelection();
        }

        public void HandleDeath(Action teamSwitchCallback, Action completeCombat)
        {
            StartCoroutine(DeathRoutine(teamSwitchCallback, completeCombat));
        }

        IEnumerator DeathRoutine(Action teamSwitchCallback, Action completeCombat)
        {
            yield return new WaitForSeconds(timingData.shakeDuration);
            
            //efekt eklenebilir + death ui
            yield return new WaitForSeconds(.3f);
            
            CommunEventbus.EffectEvents.OnDeathEffect?.Invoke(Data.UniqID);
            towerParts.RotateMiddle();
            teamSwitchCallback.Invoke();
            
            yield return new WaitForSeconds(timingData.colorFadeDuration);

            completeCombat.Invoke();
        }
        
        public void RestoreBullets() //Todo: name change: bullet hakkı
        {
            Data.BulletAmount = ConstantData.MaxBullet;
        }

        public void ResetHealth()
        {
            Data.Health = ConstantData.StartHealth;
            UIEventbus.OnHealthChange.Invoke(Data.Health, gameObject);
        }
        
        private void OnDisable()
        {
            GeneralEventbus.OnTeamChange -= FadeColor;
        }
    }
}