using System;
using System.Collections.Generic;
using Clicks;
using DataModels;
using Enums;
using GameUI;
using UnityEngine;


namespace Towers
{
    [Serializable]
    public class TowerData
    {
        public int UniqID { get; set; }
        
        public Dictionary<VisualDataType, BaseVisualSupportedData> VisualSupportedDatas = new();
        

        public bool IsClickable = true;
        public LockStatus LockStatus;

        public CombatTimingData timingData;
        public ClickHandler clickHandler;

        public TowerSegmentDataHolder SegmentData = new();
        public List<ITowerSegment> TowerSegments = new();

        public TowerMover Mover;
        public ColorHandler ColorHandler;
        public TowerUIHandler UIHandler;

        public void CreateSegmentsWithGivenVisualData()
        {
            Mover = new TowerMover(SegmentData.MoverData);
            ColorHandler = new ColorHandler(SegmentData.ColorData);
            UIHandler = new TowerUIHandler(SegmentData.UIData);

            TowerSegments.Add(Mover);
            TowerSegments.Add(ColorHandler);
            TowerSegments.Add(UIHandler);
        }

        public void CreateVisualSupportedDatas(Dictionary<VisualDataType, int> startValues)
        {
            VisualSupportedDatas.Add(VisualDataType.Shield, new ShieldData());
            VisualSupportedDatas.Add(VisualDataType.Attack, new AttackData());
            VisualSupportedDatas.Add(VisualDataType.Disarm, new DisarmData());

            foreach (var visualData in VisualSupportedDatas)
            {
                visualData.Value.Initialize(UniqID, startValues[visualData.Key]);
            }
        }

        public void SetTeamVisuals(TeamColorData teamVisualData)
        {
            ColorHandler.SetDefaultTeamVisuals(teamVisualData);
            clickHandler.SetClickableTeams(teamVisualData.TeamType);
        }

        public void EnableSelection()
        {
            if (!IsClickable) return;
            clickHandler.EnableSelection();
        }

        public void DisableSelection()
        {
            clickHandler.DisableSelection();
        }

        public void SetClickHandlerID(uint id)
        {
            clickHandler.SetClickableIds(id);
        }

        // public void UpdateHeight(int extra, TowerNumericData numericData)
        // {
        //     if (extra == 0)
        //     {
        //         Debug.Log("EQUAL");
        //         return;
        //     }
        //
        //     int newHeight = numericData.Height + extra;
        //     bool isRising = newHeight > numericData.Height;
        //     numericData.Height = newHeight;
        //
        //     Mover.ChangeHeightPhysically(newHeight, isRising);
        // }

        public void Shake()
        {
            Mover.Shake();
        }
    }

    [Serializable]
    public class TowerSegmentDataHolder
    {
        public TowerMoverData MoverData;
        public TowerColorData ColorData;
        public TowerUIData UIData;
    }
}