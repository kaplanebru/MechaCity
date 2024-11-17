using System;
using System.Collections;
using System.Linq;
using Actor;
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
        private InterruptionMotion interruptionMotion = new();

        private void OnEnable()
        {
            initializer = new TowerInitializer(this);
            Eventbus.TowerEvents.OnTurnBegin += FirstMotion;
            interruptionMotion.Subscribe();
        }

        public void Setup(TeamTowerData teamData)
        {
            initializer.Setup(teamData);
        }

        public void SetTeam(TeamTowerData teamData)
        {
            Data.TeamType = teamData.TeamType;
            Data.ColorHandler.SetTeamVisuals(teamData);
            Data.clickHandler.SetClickableTeams(teamData.TeamType);
        }

        void FirstMotion()
        {
            Data.Mover.ChangeHeightPhysically(Data.Height, true);
            StartRiseFallRoutine(true);
        }

        private Coroutine riseRoutine;

        public void StartRiseFallRoutine(bool forOnce = false)
        {
            riseRoutine = StartCoroutine(Data.Mover.riseFallMotion.RiseRoutine(forOnce));
        }

        public void StopRiseFallRoutine()
        {
            if (riseRoutine != null)
            {
                StopCoroutine(riseRoutine);
                riseRoutine = null;
            }
        }

        public void RestoreBullets() //Todo: name change: bullet hakkı
        {
            Data.BulletAmount = ConstantData.MaxBullet;
        }


        private void OnDisable()
        {
            Eventbus.TowerEvents.OnTurnBegin -= FirstMotion;
            Data.Mover.Unsubscribe();
            interruptionMotion.Unsubscribe();

        }
    }
}