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

        
       
        
        public void Setup(TeamTowerData teamTowerData)
        {
            Data.Height = ConstantData.StartHeight;
            Data.Health = ConstantData.StartHealth;
            Data.DamagePower = ConstantData.DamagePower;
            RestoreBullets();

            UIEventbus.OnHealthChange.Invoke(Data.Health, gameObject);
            Data.clickHandler.SetClickables(Data.UniqID);
            Data.BpTowerData = new BpTowerData(Data.UniqID);

            Data.mover.Initialize();
            Data.colorHandler.Initialize(Data.UniqID);
            SetTeam(teamTowerData);
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
    }
}