using System;
using System.Collections;
using Blueprint;
using Clicks;
using DataModels;
using GameUI;
using UnityEngine;


namespace Towers
{
    //[RequireComponent(typeof(TowerParts))]
    public class Tower : MonoBehaviour
    {
        //Shooter, RiserFall, Selectable Components diye 3'e ayrılabilir

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
            ColorHandler.ToOriginalColor(); //todo later
            SetTeam(teamTowerData); //for tower and clickables
        }

        public void SetTeam(TeamTowerData teamTowerData)
        {
            Data.TeamTowerData = teamTowerData;
            towerParts.FadeColor(teamTowerData.RegenerationMaterial, teamTowerData.DefaultMaterial);
            clickHandler.SetClickableTeams(teamTowerData.TeamType);
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
            
            //efekt de eklenebilir
            yield return new WaitForSeconds(.3f);
            
            teamSwitchCallback.Invoke();
            //rotate
            
            
            yield return new WaitForSeconds(timingData.colorFadeDuration + 5);

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
        
        // public void ToGivenColor(Material givenMat)
        // {
        //     towerParts.SetColor(givenMat);
        // }
    }
}