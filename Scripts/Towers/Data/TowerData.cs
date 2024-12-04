using System;
using System.Collections.Generic;
using Blueprint;
using Clicks;
using DataModels;
using Enums;
using GameUI;
using UnityEngine;


namespace Towers
{
    // [CreateAssetMenu(fileName = nameof(TowerData))]
    [Serializable]
    public class TowerData
    {
        public int UniqID;
        public int SlotId;

        //HEIGHT
        private int height;

        public int Height
        {
            get => height;
            set
            {
                height = value;
                if (!LockStatus.Locked)
                {
                    AvailableHeight = value - 1; //-1ler yeni eklendi
                }
                else
                {
                    AvailableHeight = value - 1 - LockStatus.Limit + 1; //+1 limiti sıfırlayabilmek için
                }
            }
        }

        public int AvailableHeight;

        //SHOOTING
        public bool CanShoot { get; private set; } = true;
        

        public int DamagePower;

        public TeamType TeamType;
        public List<int> NeighbourIDs = new();

        public bool IsClickable = true;
        public LockStatus LockStatus;

        public BpTowerData BpTowerData;
        public CombatTimingData timingData;
        public ClickHandler clickHandler;
        public ShieldData ShieldData = new();

        public TowerSegmentDataHolder SegmentData = new();
        public List<ITowerSegment> TowerSegments = new();

        public TowerMover Mover;
        public ColorHandler ColorHandler;
        public TowerUIHandler UIHandler;

        public void CreateSegments()
        {
            Mover = new TowerMover(SegmentData.MoverData);
            ColorHandler = new ColorHandler(SegmentData.ColorData);
            UIHandler = new TowerUIHandler(SegmentData.UIData);

            TowerSegments.Add(Mover);
            TowerSegments.Add(ColorHandler);
            TowerSegments.Add(UIHandler);
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

        public void UpdateHeight(int extra)
        {
            if (extra == 0)
            {
                Debug.Log("EQUAL");
                return;
            }

            int newHeight = Height + extra;
            bool isRising = newHeight > Height;
            Height = newHeight;

            Mover.ChangeHeightPhysically(newHeight, isRising);
        }

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