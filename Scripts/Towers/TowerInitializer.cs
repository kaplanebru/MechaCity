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
            _tower.RestoreBullets();

            UIEventbus.OnHealthChange.Invoke(Data.Health, _tower.gameObject);
            Data.clickHandler.SetClickables(Data.UniqID);
            Data.BpTowerData = new BpTowerData(Data.UniqID);
            
            SetSegments();
            _tower.SetTeam(teamData);
        }
        void SetSegments()
        {
            Data.TowerSegments = _tower.gameObject.GetComponentsInChildren<ITowerSegment>();
            foreach (var segment in Data.TowerSegments )
            {
                segment.SetId(Data.UniqID);
                segment.Initialize();
            }
        }
    }

}
