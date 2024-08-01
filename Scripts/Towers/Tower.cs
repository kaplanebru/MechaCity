using System;
using System.Collections;
using System.Linq;
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
        private TowerInitializer initializer;

        private void OnEnable()
        {
            initializer = new TowerInitializer(this);
            Eventbus.TowerEvents.OnTurnBegin += FirstMotion;
        }
        
        public void Setup(TeamTowerData teamData)
        {
            initializer.Setup(teamData);
        }

        IEnumerator LoadDelay(TeamTowerData teamData)
        {
            yield return new WaitForSeconds(1);
            initializer.Setup(teamData);
        }
        
        public void SetTeam(TeamTowerData teamData)
        {
            Data.TeamType = teamData.TeamType;
            Data.ColorHandler.SetTeamVisuals(teamData);
            Data.clickHandler.SetClickableTeams(teamData.TeamType);
        }
        public void EnableSelection()
        {
            if (!Data.IsClickable) return;
            Data.clickHandler.EnableSelection();
        }

        public void DisableSelection()
        {
            Data.clickHandler.DisableSelection();
        }

        public void HandleDeath(Action teamSwitchCallback, Action completeCombat)
        {
            StartCoroutine(DeathRoutine(teamSwitchCallback, completeCombat));
        }

        IEnumerator DeathRoutine(Action teamSwitchCallback, Action completeCombat)
        {
            yield return new WaitForSeconds(Data.timingData.shakeDuration);

            yield return new WaitForSeconds(.3f);

            MediatorEventbus.EffectEvents.OnDeathEffect?.Invoke(Data.UniqID);
            Data.Mover.RotateMiddle();
            teamSwitchCallback.Invoke();

            yield return new WaitForSeconds(Data.timingData.colorFadeDuration);

            completeCombat.Invoke();
        }

        void FirstMotion()
        {
           Data.Mover.ChangeHeight(Data.Height, true);
           StartRiseFallRoutine();
           Invoke(nameof(StopRiseFallRoutine), 1); //TODO: add start time
        }

        public void StartRiseFallRoutine()
        {
            StartCoroutine(Data.Mover.riseFallMotion.RiseRoutine());
        }

        public void StopRiseFallRoutine()
        {
            StopCoroutine(Data.Mover.riseFallMotion.RiseRoutine());
        }

        public void RestoreBullets() //Todo: name change: bullet hakkı
        {
            Data.BulletAmount = ConstantData.MaxBullet;
        }

        public void ResetHealth()
        {
            Data.Health = ConstantData.StartHealth;
            UIEventbus.OnHealthChange.Invoke(Data.Health, Data.UniqID);
        }

        private void OnDisable()
        {
            Eventbus.TowerEvents.OnTurnBegin -= FirstMotion;
        }
    }
}