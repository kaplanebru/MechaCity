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

        public TowerMover mover;
        public TowerVisuals visuals;
        public ClickHandler clickHandler;

        private void OnEnable()
        {
            //GeneralEventbus.OnTeamChange += FadeColor;
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

            mover.Initialize();
            visuals.Initialize();
            SetTeam(teamTowerData);
        }

        public void SetTeam(TeamTowerData teamTowerData)
        {
            visuals.Data.TeamData = teamTowerData;
            Data.TeamType = teamTowerData.TeamType;
            // GeneralEventbus.OnTeamChange.Invoke(Data.UniqID); 
            visuals.FadeColor();

            clickHandler.SetClickableTeams(teamTowerData.TeamType);
        }

        // void FadeColor(int id)
        // {
        //     if (id != Data.UniqID) return;
        //     visuals.FadeColor();
        // }

        public void EnableSelection()
        {
            if (!Data.IsClickable) return;
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

            yield return new WaitForSeconds(.3f);

            CommunEventbus.EffectEvents.OnDeathEffect?.Invoke(Data.UniqID);
            mover.RotateMiddle();
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
            //GeneralEventbus.OnTeamChange -= FadeColor;
        }
    }
}