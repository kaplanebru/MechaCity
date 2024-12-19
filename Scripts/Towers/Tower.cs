using System.Collections;
using System.Linq;
using Actor;
using Clicks;
using DataModels;
using DG.Tweening;
using GameUI;
using UnityEngine;


namespace Towers
{

    public class Tower : MonoBehaviour
    {
        public TowerConstantData ConstantData;
        public TowerData Data;
        public TowerNumericData NumericData;
        public TowerInitializer initializer;
        private InterruptionMotion interruptionMotion = new();

        private void OnEnable()
        {
            Eventbus.TowerEvents.OnTurnBegin += FirstMotion;
            interruptionMotion.Subscribe();
        }
        
        void FirstMotion()
        {
            Data.Mover.ChangeHeightPhysically(NumericData.Height, true);
            StartRiseFallRoutine(true);
        }

        private Coroutine riseRoutine;

        public void StartRiseFallRoutine(bool forOnce = false)
        {
            riseRoutine = StartCoroutine(Data.Mover.riseFallMotion.RiseRoutine(forOnce));
        }

        public void StopRiseFallRoutine()
        {
            if (riseRoutine != null)
            {
                StopCoroutine(riseRoutine);
                riseRoutine = null;
            }
        }
        private void OnDisable()
        {
            Eventbus.TowerEvents.OnTurnBegin -= FirstMotion;
            Data.Mover.Unsubscribe();
            interruptionMotion.Unsubscribe();
        }
        
        public void UpdateHeight(int extra)
        {
            if (extra == 0)
            {
                Debug.Log("EQUAL");
                return;
            }

            int newHeight = NumericData.Height + extra;
            bool isRising = newHeight > NumericData.Height;
            NumericData.Height = newHeight;

            Data.Mover.ChangeHeightPhysically(newHeight, isRising);
        }
    }
}