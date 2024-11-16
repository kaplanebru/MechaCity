using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;

namespace Curves
{
    public class IndicatorData
    {
        public Vector3[] Points;
        public IndicatorState State;
    }
    public class IndicatorController : MonoBehaviour
    {
        public CurveData curveData;
        
        [SerializeField]private Gradient[] gradients;
        private Dictionary<IndicatorState, Gradient> gradientByState = new();

        private LineCreator lineCreator;
        private CurvePointCreator pointCreator;

        private Dictionary<uint, List<IndicatorData>> indicatorsByActor = new();

        private void OnEnable()
        {
            pointCreator = new(curveData.EdgeDistance, curveData.PointDistance, curveData.HeightOffset);
            lineCreator = new LineCreator(curveData.LineRenderers);
            SetGradientsByState();
            
            SubscribePermanently();
            HideLines();
        }
        private void SubscribePermanently()
        {
            Eventbus.TurnStateEvents.OnTurnStateBegin += HoverEnable;
            IndicatorEvents.OnIndicatorGridDatasSet += SetIndicators;
            
            GeneralEventbus.IndicatorEvents.OnActorHoverByCombat += ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorLeftByCombat += HideLines;
        }
        
        public void Subscribe()
        {
            GeneralEventbus.IndicatorEvents.OnActorHoverByUser += ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorLeftByUser += HideLines;
        }

        void SetGradientsByState()
        {
            gradientByState.Add(IndicatorState.Enemy, gradients[0]);
            gradientByState.Add(IndicatorState.Friendly, gradients[1]);
        }
        private void SetIndicators(IndicatorGridData[] gridDatas)
        {
            indicatorsByActor.Clear();
            foreach (var gridData in gridDatas)
            {
                indicatorsByActor.Add(gridData.ActorID, new List<IndicatorData>());
                foreach (var targetPos in gridData.TargetPositions)
                {
                    var indicatorData = new IndicatorData();
                    indicatorData.Points = pointCreator.GetCurvePoints(gridData.StartPos, targetPos).ToArray();
                    indicatorData.State = gridData.TargetStates[targetPos];
                    
                    indicatorsByActor[gridData.ActorID].Add(indicatorData);
                }
            }
        }
        void ShowLinesByActor(uint actorID)
        {
            if(!indicatorsByActor.ContainsKey(actorID)) return;

            var indicators = indicatorsByActor[actorID];
            for (int i = 0; i < indicators.Count; i++)
            {
                lineCreator.PointsToLinesWithColor(i, indicators[i].Points, gradientByState[indicators[i].State]);
            }
        }
        
        private void HoverEnable(TurnStateType turnState)
        {
            Debug.Log(turnState);
            if(turnState == TurnStateType.Selection)
                Subscribe();
            else
                Unsubscribe();
        }
        
        private void HideLines()
        {
            lineCreator.DisableLines();
        }

        public void Unsubscribe()
        {
            GeneralEventbus.IndicatorEvents.OnActorHoverByUser -= ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorLeftByUser -= HideLines;
        }
        private void UnsubscribePermanently()
        {
            Eventbus.TurnStateEvents.OnTurnStateBegin -= HoverEnable;
            IndicatorEvents.OnIndicatorGridDatasSet -= SetIndicators;
            
            GeneralEventbus.IndicatorEvents.OnActorHoverByCombat -= ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorLeftByCombat -= HideLines;
        }
        
        private void OnDisable()
        {
            Unsubscribe();
            UnsubscribePermanently();
        }
    }

    public enum CurveDirection
    {
        Right,
        Left
    }
}