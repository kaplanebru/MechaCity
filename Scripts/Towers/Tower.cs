using System;
using System.Collections;
using Blueprint;
using Clicks;
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
        public TowerParts towerParts;
        public ClickHandler clickHandler;


        private void OnEnable()
        {
            towerParts = GetComponent<TowerParts>();
            clickHandler = GetComponent<ClickHandler>();
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
            SetTeam(teamTowerData); //for tower and clickables
        }

        public void SetTeam(TeamTowerData teamTowerData)
        {
            Data.TeamTowerData = teamTowerData;
            ToOriginalColor();
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
            ToDeadColor();

            yield return new WaitForSeconds(1);
            
            teamSwitchCallback.Invoke();
            
            yield return new WaitForSeconds(1);

            completeCombat.Invoke();
        }

        public void ToFreezeColor()
        {
            towerParts.SetColor(Data.TeamTowerData.FreezeMaterial);
        }

        public void ToBlueprintColor()
        {
            towerParts.SetColor(Data.TeamTowerData.BlueprintMaterial);
        }

        public void ToSelectionColor()
        {
            towerParts.SetColor(Data.TeamTowerData.SelectedMaterial);
        }

        public void ToOriginalColor()
        {
            towerParts.SetColor(Data.TeamTowerData.DefaultMaterial);
        }

        public void ToDeadColor()
        {
            print("dead mat");
            towerParts.SetColor(Data.TeamTowerData.DeadMaterial);
        }

        public void ToRegenerationColor()
        {
            towerParts.SetColor(Data.TeamTowerData.RegenerationMaterial);
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