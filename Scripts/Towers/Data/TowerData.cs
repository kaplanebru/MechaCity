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
        public int Height;
        public int SlotId;
        public TeamType TeamType;
        public List<int> LinkedTowerIDs = new();
        public bool CanShoot { get; private set; }
        
        [SerializeField] private int _bulletAmountt = 1;
        public int BulletAmount
        {
            get => _bulletAmountt;
            set
            {
                _bulletAmountt = value;
                CanShoot = value > 0;
            }
        }
        
 

        private int _combatStep;
        public int CombatStep
        {
            get;
            set;
        }

        [SerializeField] int _health = 1;

        public int Health
        {
            get => _health;
            set => _health = value;
        }

        public int DamagePower;
        public bool IsClickable = true;
        public LockStatus LockStatus;
        
        public BpTowerData BpTowerData;
        public CombatTimingData timingData;
        public ClickHandler clickHandler;
        
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
    }

    [Serializable]
    public class TowerSegmentDataHolder
    {
        public TowerMoverData MoverData;
        public TowerColorData ColorData;
        public TowerUIData UIData;
    }
}


