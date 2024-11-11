using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Curves
{
    public class IndicatorController : MonoBehaviour
    {
        public IndicatorData Data;
        
        private LineCreator lineCreator;
        private CurvePointCreator pointCreator;
        
        private Dictionary<uint, List<PointGroup>> pointGroupsByActor = new();
        private uint currentActor;

        private void OnEnable()
        {
            Subscribe();
        }

        public void Subscribe()
        {
            pointCreator = new(Data.EdgeDistance, Data.PointDistance, Data.HeightOffset);
            lineCreator = new LineCreator(Data.LineRenderers);

            GeneralEventbus.IndicatorEvents.OnActorsResolved += SetPointGroupsByActors;
            GeneralEventbus.IndicatorEvents.OnActorHover += ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnLeavingActor += HideLines;
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
                    pointGroupsByActor[actorAndEdges.Key].Add(new PointGroup(i, pointCreator.GetCurvePoints(start, end).ToArray()));
                }
            }
        }

        private bool IsActorSame(uint actorID)
        {
            if (actorID == currentActor)
            {
                lineCreator.EnableLines(pointGroupsByActor[actorID].Count);
                return true;
            }
            return false;
        }

        private void ShowLinesByActor(uint actorID)
        {
            if (!pointGroupsByActor.ContainsKey(actorID)) return;
            if (IsActorSame(actorID)) return;
            currentActor = actorID;

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
            GeneralEventbus.IndicatorEvents.OnActorHover -= ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorsResolved -= SetPointGroupsByActors;
            GeneralEventbus.IndicatorEvents.OnLeavingActor -= HideLines;
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

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