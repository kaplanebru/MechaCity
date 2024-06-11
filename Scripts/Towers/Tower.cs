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
        public CombatTimingData timingData;

        private void OnEnable()
        {
            UIEventbus.OnTowerHeightChange += UIHeightChangeRequest;
        }

        private void UIHeightChangeRequest(float height, int id)
        {
            if(Data.UniqID != id) return;
            Data.uiHandler.ChangeHeightUI(height);
        }
        public void Setup(TeamTowerData teamTowerData)
        {
            Data.Height = ConstantData.StartHeight;
            Data.Health = ConstantData.StartHealth;
            Data.DamagePower = ConstantData.DamagePower;
            RestoreBullets();

            UIEventbus.OnHealthChange.Invoke(Data.Health, gameObject);
            Data.clickHandler.SetClickables(Data.UniqID);
            Data.BpTowerData = new BpTowerData(Data.UniqID);
            
            SetSegments();
            SetTeam(teamTowerData);
        }
        
        void SetSegments()
        {
            Data.TowerSegments = GetComponentsInChildren<ITowerSegment>();

            foreach (var segment in Data.TowerSegments )
            {
                segment.SetId(Data.UniqID);
                segment.Initialize();
            }
        }

        public void SetTeam(TeamTowerData teamData)
        {
            Data.TeamType = teamData.TeamType;
            
            Data.colorHandler.SetTeamVisuals(teamData);
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
            yield return new WaitForSeconds(timingData.shakeDuration);

            yield return new WaitForSeconds(.3f);

            CommunEventbus.EffectEvents.OnDeathEffect?.Invoke(Data.UniqID);
            Data.mover.RotateMiddle();
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
            UIEventbus.OnTowerHeightChange -= UIHeightChangeRequest;
        }
    }
}