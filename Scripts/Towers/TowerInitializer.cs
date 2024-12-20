using System.Collections;
using System.Collections.Generic;
using Enums;
using GameUI;
using UnityEngine;

namespace Towers
{
    public class TowerInitializer
    {
        private TowerObject _towerObject;
        private TowerConstantData ConstantData;
        private TowerData InclusiveData;
        private TowerNumericData NumericData;

        public TowerInitializer(TowerObject towerObject)
        {
            _towerObject = towerObject;
            ConstantData = _towerObject.ConstantData;
            NumericData = towerObject.Data.NumericData;
            InclusiveData = _towerObject.Data;
        }

        public void NumericDataInitialSetup(TeamType teamType)
        {
            NumericData.LockStatus = ConstantData.StartLockStatus;
            NumericData.Height = ConstantData.StartHeight;
            NumericData.TeamType = teamType;
            NumericData.ShotAmount = ConstantData.ShotAmount;
            NumericData.ShieldHeight = ConstantData.ShieldHeight;
            NumericData.DamagePower = ConstantData.DamagePower;

            InclusiveData.VisualData.UniqID = NumericData.UniqID;
        }

        public void VisualDataIdentification()
        {
            InclusiveData.VisualData.CreateSegmentsWithGivenVisualData();
        }

        public void VisualDataInitialSetup(TeamColorData teamData)
        {
            SetSegments();
            InclusiveData.VisualData.SetTeamVisuals(teamData);
        }

        public void TowerBPElementsDataSetup()
        {
            InclusiveData.VisualData.CreateVisualSupportedDatas(
                new Dictionary<VisualDataType, int> //TODO: Bunlar bp trigger edilerek de yapılabilir
                {
                    {VisualDataType.Shield, ConstantData.ShieldHeight},
                    {VisualDataType.Attack, ConstantData.ShotAmount},
                    {VisualDataType.Disarm, ConstantData.IsDisarmed ? 0 : 1}
                });
        }

        void SetSegments()
        {
            foreach (var segment in InclusiveData.VisualData.TowerSegments)
            {
                segment.SetId(NumericData.UniqID);
                segment.Initialize();
            }
        }

        public void SetTowerRelatedIds()
        {
            var towerRelations = _towerObject.GetComponentsInChildren<ITowerRelatedElement>();
            foreach (var related in towerRelations)
            {
                related.Initialize(NumericData.UniqID);
            }
        }

        public void ExecuteVisualsAfterSetup()
        {
            foreach (var visualSupportedData in InclusiveData.VisualData.VisualSupportedDatas)
            {
                visualSupportedData.Value.SetVisually();
            }
        }
    }
}