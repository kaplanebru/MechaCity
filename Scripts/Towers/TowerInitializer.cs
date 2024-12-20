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
        private TowerVisualData visualData;
        private TowerNumericData NumericData;

        public TowerInitializer(TowerObject towerObject)
        {
            this.towerObject = towerObject;
            ConstantData = this.towerObject.ConstantData;
            NumericData = towerObject.Data.NumericData;
            visualData = this.towerObject.Data.VisualData;
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
            visualData.UniqID = NumericData.UniqID;
        }

        public void VisualDataIdentification()
        {
            visualData.CreateSegmentsWithGivenVisualData();
        }

        public void VisualDataInitialSetup(TeamColorData teamData)
        {
            SetSegments();
            visualData.SetTeamVisuals(teamData);
        }

        public void TowerBPElementsDataSetup()
        {
            visualData.CreateVisualSupportedDatas(
                new Dictionary<VisualDataType, int> //TODO: Bunlar bp trigger edilerek de yapılabilir
                {
                    {VisualDataType.Shield, ConstantData.ShieldHeight},
                    {VisualDataType.Attack, ConstantData.ShotAmount},
                    {VisualDataType.Disarm, ConstantData.IsDisarmed ? 0 : 1}
                });
        }

        void SetSegments()
        {
            foreach (var segment in visualData.TowerSegments)
            {
                segment.SetId(visualData.UniqID);
                segment.Initialize();
            }
        }

        public void SetTowerRelatedIds()
        {
            var towerRelations = towerObject.GetComponentsInChildren<ITowerRelatedElement>();
            foreach (var related in towerRelations)
            {
                related.Initialize(visualData.UniqID);
            }
        }

        public void ExecuteVisualsAfterSetup()
        {
            foreach (var visualSupportedData in visualData.VisualSupportedDatas)
            {
                visualSupportedData.Value.SetVisually();
            }
        }
    }
}