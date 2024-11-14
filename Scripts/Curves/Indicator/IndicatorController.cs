using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Enums;

namespace Curves
{
    public class IndicatorController : MonoBehaviour
    {
        public IndicatorData Data;

        private LineCreator lineCreator;
        private CurvePointCreator pointCreator;

        private Dictionary<uint, List<PointGroup>> pointGroupsByActor = new();


        private void OnEnable()
        {
            pointCreator = new(Data.EdgeDistance, Data.PointDistance, Data.HeightOffset);
            lineCreator = new LineCreator(Data.LineRenderers);
            
            SubscribePermanently();
            HideLines();
            
        }
        private void SubscribePermanently()
        {
            Eventbus.TurnStateEvents.OnTurnStateBegin += HoverEnable;
            
            GeneralEventbus.IndicatorEvents.OnActorsEdgesRestored += SetPointGroupsByActors;
            GeneralEventbus.IndicatorEvents.OnActorHoverByCombat += ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorLeftByCombat += HideLines;
        }

        private void HoverEnable(TurnStateType turnState)
        {
            Debug.Log(turnState);
            if(turnState == TurnStateType.Selection)
                Subscribe();
            else
                Unsubscribe();
            
        }

        public void Subscribe()
        {
            GeneralEventbus.IndicatorEvents.OnActorHoverByUser += ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorLeftByUser += HideLines;
        }
        private void EnableIndicatorHover()
        {
            Subscribe();
        }

        private void DisableIndicatorHover()
        {
            Unsubscribe();
        }
        private void SetPointGroupsByActors(Dictionary<uint, List<Vector3>> actorsAndEdgesData)
        {
            pointGroupsByActor.Clear();

            foreach (var actorAndEdges in actorsAndEdgesData)
            {
                var start = actorAndEdges.Value[0];
                pointGroupsByActor.Add(actorAndEdges.Key, new List<PointGroup>());

                for (var i = 1; i < actorAndEdges.Value.Count; i++)
                {
                    var end = actorAndEdges.Value[i];
                    pointGroupsByActor[actorAndEdges.Key]
                        .Add(new PointGroup(i-1, pointCreator.GetCurvePoints(start, end).ToArray()));
                }
            }
        }
        
        private void ShowLinesByActor(uint actorID)
        {
            if (!pointGroupsByActor.ContainsKey(actorID)) return;


            foreach (var pointGroup in pointGroupsByActor[actorID])
            {
                lineCreator.PointsToLines(pointGroup.Index, pointGroup.Points);
            }
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


            
            GeneralEventbus.IndicatorEvents.OnActorsEdgesRestored -= SetPointGroupsByActors;
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