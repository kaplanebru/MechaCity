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
        
        public void DataSetup()
        {
            Data.Height = ConstantData.StartHeight;
            Data.DamagePower = ConstantData.DamagePower;
            Data.LockStatus = ConstantData.StartLockStatus;
            Data.BpTowerData = new BpTowerData(Data.UniqID);
        }

        public void DataVisualCorrespondenceSetup(TeamColorData teamData)
        {
            Data.CreateSegments();
            SetSegments();
            _tower.SetTeamVisuals(teamData);
        }

        public void TowerBPElementsDataSetup()
        {
            Data.CreateVisualSupportedDatas(new Dictionary<VisualDataType, int> //TODO: Bunlar bp trigger edilerek de yapılabilir
            {
                { VisualDataType.Shield, ConstantData.ShieldHeight },
                { VisualDataType.Attack, ConstantData.ShotAmount },
                { VisualDataType.Disarm, ConstantData.IsDisarmed ? 0 : 1}
            });
        }
        void SetSegments()
        {
            foreach (var segment in Data.TowerSegments)
            {
                segment.SetId(Data.UniqID);
                segment.Initialize();
            }
        }

        public void SetTowerRelatedIds()
        {
            var towerRelations =_tower.GetComponentsInChildren<ITowerRelatedElement>();
            foreach (var related in towerRelations)
            {
                related.Initialize(Data.UniqID);
            }
        }

        public void ExecuteVisualsAfterSetup()
        {
            foreach (var visualSupportedData in Data.VisualSupportedDatas)
            {
                visualSupportedData.Value.SetVisually();
            }
        }

    }

}
