using System;
using Enums;
using UnityEngine;


namespace Clicks
{
    public class Clickable : BaseClickable<int> //uniqID
    {
        public uint id;
        public TeamType teamType;
        private Collider _collider;
        public IndicatorScanner indicatorScanner;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            indicatorScanner = GetComponent<IndicatorScanner>();
            // _collider.enabled = false;
            
        }

        public void SetID(uint Id)
        {
            id = Id;
            SetIndicator();
        }
        
        public void SetIndicator()
        {
            indicatorScanner.Setup(id);
        }

       
        protected override void Setup() {}

        public override void UnsubscribeFromEvent() {}
    }

}
