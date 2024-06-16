using System.Collections;
using System.Collections.Generic;
using Blueprint;
using GameUI;
using UnityEngine;

namespace Towers
{
    public class TowerInitializer
    {
        private Tower _tower;
        private TowerConstantData ConstantData;
        private TowerData Data;
        
        public TowerInitializer(Tower tower)
        {
            _tower = tower;
            ConstantData = _tower.ConstantData;
            Data = _tower.Data;
        }
        
        public void Setup(TeamTowerData teamData)
        {
            Data.Height = ConstantData.StartHeight;
            Data.Health = ConstantData.StartHealth;
            Data.DamagePower = ConstantData.DamagePower;
            Data.LockStatus = ConstantData.StartLockStatus;
            
            _tower.RestoreBullets();
            Data.BpTowerData = new BpTowerData(Data.UniqID);
            
            Data.CreateSegments();
            SetSegments();
            SetTowerRelatedIds();
            
            _tower.SetTeam(teamData);
            
            UIEventbus.OnHealthChange.Invoke(Data.Health, Data.UniqID);
            Data.Mover.SetHeight(1);
        }
        void SetSegments()
        {
            foreach (var segment in Data.TowerSegments )
            {
                segment.SetId(Data.UniqID);
                segment.Initialize();
            }
        }

        void SetTowerRelatedIds()
        {
            var towerRelations =_tower.GetComponentsInChildren<ITowerRelated>();
            foreach (var related in towerRelations)
            {
                related.Initialize(Data.UniqID);
            }
        }
    }

}
