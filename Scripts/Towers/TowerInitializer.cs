using System.Collections;
using System.Collections.Generic;
using Blueprint;
using Enums;
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
        
        public void Setup(TeamColorData teamData)
        {
            Data.Height = ConstantData.StartHeight;
            Data.DamagePower = ConstantData.DamagePower;
            Data.LockStatus = ConstantData.StartLockStatus;
            Data.BpTowerData = new BpTowerData(Data.UniqID);
            
            Data.CreateSegments();
            Data.CreateVisualSupportedDatas(new Dictionary<VisualDataType, int> //TODO: Bunlar bp trigger edilerek de yapılamaz, user lazım
            {
                { VisualDataType.Shield, ConstantData.ShieldHeight },
                { VisualDataType.Attack, ConstantData.ShotAmount },
                { VisualDataType.Disarm, ConstantData.IsDisarmed ? 0 : 1}
            });
            
            SetSegments();
            SetTowerRelatedIds();
            
            _tower.SetTeam(teamData);
            
            //UIEventbus.OnHealthChange.Invoke(Data.Health, Data.UniqID);
           // Data.Mover.riseFallMotion.SetZeroHeight(0); //warning: bug sebebi (0'la başlarsa y<1 olur ve ekstra passive part açar
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

        public void ExecuteAfterSetup()
        {
            foreach (var visualSupportedData in Data.VisualSupportedDatas)
            {
                visualSupportedData.Value.SetVisually();
            }
        }

    }

}
