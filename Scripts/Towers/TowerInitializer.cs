using System.Collections;
using System.Collections.Generic;
using Enums;
using GameUI;
using UnityEngine;

namespace Towers
{
    public class TowerInitializer
    {
        private TowerObject towerObject;
        private TowerConstantData ConstantData;
        private TowerData Data;
        private TowerNumericData NumericData;

        public TowerInitializer(TowerObject towerObject)
        {
            this.towerObject = towerObject;
            ConstantData = this.towerObject.ConstantData;
            NumericData = towerObject.NumericData;
            Data = this.towerObject.Data;
        }

        public void NumericDataInitialSetup(TeamType teamType)
        {
            NumericData.Height = ConstantData.StartHeight;
            NumericData.TeamType = teamType;
            NumericData.LockStatus = ConstantData.StartLockStatus;
            NumericData.ShotAmount = ConstantData.ShotAmount;
            NumericData.ShieldHeight = ConstantData.ShieldHeight;
            NumericData.DamagePower = ConstantData.DamagePower;

            // Data.Height = ConstantData.StartHeight;
            Data.UniqID = NumericData.UniqID;
        }

        public void VisualDataIdentification()
        {
            Data.CreateSegmentsWithGivenVisualData();
        }

        public void VisualDataInitialSetup(TeamColorData teamData)
        {
            SetSegments();
            Data.SetTeamVisuals(teamData);
        }

        public void TowerBPElementsDataSetup()
        {
            Data.CreateVisualSupportedDatas(
                new Dictionary<VisualDataType, int> //TODO: Bunlar bp trigger edilerek de yapılabilir
                {
                    {VisualDataType.Shield, ConstantData.ShieldHeight},
                    {VisualDataType.Attack, ConstantData.ShotAmount},
                    {VisualDataType.Disarm, ConstantData.IsDisarmed ? 0 : 1}
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
            var towerRelations = towerObject.GetComponentsInChildren<ITowerRelatedElement>();
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