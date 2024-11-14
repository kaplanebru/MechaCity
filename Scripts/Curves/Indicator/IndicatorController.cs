using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;

namespace Curves
{
    public class IndicatorData
    {
        //public int Index;
        public Vector3[] Points;
        public IndicatorState State;
    }
    public class IndicatorController : MonoBehaviour
    {
        public CurveData curveData;

        private LineCreator lineCreator;
        private CurvePointCreator pointCreator;

        private Dictionary<uint, List<PointGroup>> pointGroupsByActor = new();
        private Dictionary<uint, List<IndicatorData>> indicatorsByActor = new();


        private void OnEnable()
        {
            pointCreator = new(curveData.EdgeDistance, curveData.PointDistance, curveData.HeightOffset);
            lineCreator = new LineCreator(curveData.LineRenderers);
            
            SubscribePermanently();
            HideLines();
            
        }
        private void SubscribePermanently()
        {
            Eventbus.TurnStateEvents.OnTurnStateBegin += HoverEnable;
            IndicatorEvents.OnIndicatorGridDatasSet += SetIndicators;
            
            //GeneralEventbus.IndicatorEvents.OnActorsEdgesRestored += SetPointGroupsByActors;
            GeneralEventbus.IndicatorEvents.OnActorHoverByCombat += ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorLeftByCombat += HideLines;
        }

        private void SetIndicators(List<IndicatorGridData> gridDatas)
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
                
                lineCreator.PointsToLines(i, indicators[i].Points);
            }
        }
        
        // private void ShowLinesByActor(uint actorID)
        // {
        //     if (!pointGroupsByActor.ContainsKey(actorID)) return;
        //
        //
        //     foreach (var pointGroup in pointGroupsByActor[actorID])
        //     {
        //         lineCreator.PointsToLines(pointGroup.Index, pointGroup.Points);
        //     }
        // }

        // private void SetPointGroupsByActors(Dictionary<uint, List<Vector3>> actorsAndEdgesData)
        // {
        //     pointGroupsByActor.Clear();
        //
        //     foreach (var actorAndEdges in actorsAndEdgesData)
        //     {
        //         var start = actorAndEdges.Value[0];
        //         pointGroupsByActor.Add(actorAndEdges.Key, new List<PointGroup>());
        //
        //         for (var i = 1; i < actorAndEdges.Value.Count; i++)
        //         {
        //             var end = actorAndEdges.Value[i];
        //             pointGroupsByActor[actorAndEdges.Key]
        //                 .Add(new PointGroup(i-1, pointCreator.GetCurvePoints(start, end).ToArray()));
        //         }
        //     }
        // }

        public void Subscribe()
        {
            GeneralEventbus.IndicatorEvents.OnActorHoverByUser += ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorLeftByUser += HideLines;
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
            
            
            //GeneralEventbus.IndicatorEvents.OnActorsEdgesRestored -= SetPointGroupsByActors;
            GeneralEventbus.IndicatorEvents.OnActorHoverByCombat -= ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorLeftByCombat -= HideLines;
        }
        
        private void OnDisable()
        {
            Unsubscribe();
            UnsubscribePermanently();
        }

        #region Same

        // private bool IsActorSame(uint actorID)
        // {
        //     if (actorID == currentActor)
        //     {
        //         lineCreator.EnableLines(pointGroupsByActor[actorID].Count);
        //         return true;
        //     }
        //     return false;
        // }

        #endregion

        #region Tiling

        // void TilingDots()
        // {
        //     Material lineMaterial = new Material(Shader.Find("Unlit/Transparent")); 
        //     lineMaterial.mainTexture = texture;
        //     lr.material = lineMaterial;
        //     float textureRepeat = lr.positionCount/3f;
        //     lr.material.mainTextureScale = new Vector2(textureRepeat, 1);
        // }

        #endregion
    }

    public enum CurveDirection
    {
        Right,
        Left
    }
}