using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace UX
{
    public class PointGroup
    {
        public int Index;
        public Vector3[] Points;

        public PointGroup(int index, Vector3[] points)
        {
            Index = index;
            Points = points;
        }
    }

    public class Indicator : MonoBehaviour
    {
        public LineRenderer[] lineRenderers;
        public float pointDistance = 0.1f;
        public float edgeDistance = 3;
        public float heightOffset = 2;

        private Dictionary<uint, List<PointGroup>> pointGroupsByActor = new();
        private uint currentActor;

        private CurvePointCreator pointCreator;
        private Vector3[] linePoints;

        private void OnEnable()
        {
            pointCreator = new(edgeDistance, pointDistance, heightOffset);

            GeneralEventbus.IndicatorEvents.OnActorsResolved += RestorePointGroups;
            GeneralEventbus.IndicatorEvents.OnActorHover += ShowLinesByActor;
        }

        private void RestorePointGroups(Dictionary<uint, List<Vector3>> actorsAndEdgesData)
        {
            pointGroupsByActor.Clear();
         
            foreach (var actorAndEdges in actorsAndEdgesData)
            {
                var start = actorAndEdges.Value[0];
                pointGroupsByActor.Add(actorAndEdges.Key, new List<PointGroup>());
                
                for (var i = 1; i < actorAndEdges.Value.Count; i++)
                {
                    var end = actorAndEdges.Value[i];

                    pointCreator.Setup(start, end);
                    pointGroupsByActor[actorAndEdges.Key].Add(new PointGroup(i, pointCreator.GetCurvePoints().ToArray()));
                }
                
            }
        }

        private bool IsActorSame(uint actorID)
        {
            if (actorID == currentActor)
            {
                int lineAmount = pointGroupsByActor[actorID].Count;
                for (int i = 0; i < lineAmount; i++)
                {
                    lineRenderers[i].enabled = true;
                }

                return true;
            }

            return false;
        }

        private void ShowLinesByActor(uint actorID)
        {
            if (!pointGroupsByActor.ContainsKey(actorID)) return;
            Debug.Log("show: " + actorID);

            if (IsActorSame(actorID)) return;
            currentActor = actorID;

            foreach (var pointGroup in pointGroupsByActor[actorID])
            {
                PointsToLines(pointGroup.Index, pointGroup.Points);
                Debug.Log(pointGroup.Points[0]);
            }
        }

        private void PointsToLines(int index, Vector3[] points)
        {
            var lr = lineRenderers[index]; //todo test
            lr.enabled = true;
            lr.positionCount = 0;
            lr.positionCount = points.Length;
            lr.SetPositions(points);
        }

        private void OnDisable()
        {
            GeneralEventbus.IndicatorEvents.OnActorHover -= ShowLinesByActor;
            GeneralEventbus.IndicatorEvents.OnActorsResolved -= RestorePointGroups;
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