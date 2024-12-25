using System.Collections;
using System.Linq;
using Clicks;
using DataModels;
using DG.Tweening;
using GameUI;
using UnityEngine;


namespace Towers
{

    public class TowerObject : MonoBehaviour
    {
        public TowerConstantData ConstantData;
        public TowerData Data;
        public TowerInitializer initializer;

        private void OnEnable()
        {
            Eventbus.TowerEvents.OnTurnBegin += FirstMotion;
        }
        
        void FirstMotion()
        {
            Data.VisualData.Mover.SetHeightPhysically(Data.NumericData.Height, true);
            //Data.SetHeight(Data.NumericData.Height);
            StartRiseFallRoutine(true);
        }
        

        private Coroutine riseRoutine = null;

        public void StartRiseFallRoutine(bool forOnce = false)
        {
            //if(riseRoutine != null) return; //ya da stop start, todo: check
            riseRoutine = StartCoroutine(Data.VisualData.Mover.riseFallMotion.RiseRoutine(forOnce));
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
            Data.VisualData.Mover.Unsubscribe();
        }
    }
}