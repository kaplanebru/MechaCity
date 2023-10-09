using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Enums;
using Unity.Collections;
using UnityEngine;

//DISTANCE BASED
namespace Chain
{
    [RequireComponent(typeof(ChainDrawer))]
    public class ChainSpawner : MonoBehaviour
    {
        public ChainDrawer chainDrawer;
        public ArcPart[] arcParts;
        public float unit = 1;
        public int linearPointAmount = 1;

        private Vector3 unitDistance;
        ChainState state;
        [ReadOnly] public List<Vector3> chainPoints = new();

        private void Start()
        {
            chainPoints.Clear();
            state = arcParts.Length % 2 == 0 ? ChainState.Even : ChainState.Odd;


            SetCircularPoints();
            SetLinearPoints();
            BindPoints();
        }

        private Vector3 center;

        void GetCenter()
        {
            center = ChainHelper.CenterDirection(arcParts);
        }

        void SetCircularPoints()
        {
            GetCenter();
            for (int i = 0; i < arcParts.Length; i++)
            {
                SetArcRotation(i);
                CreateHalfCircleByAngle(i);
                RotatePoints(i);
            }
        }

        void SetArcRotation(int i)
        {
            var mainArc = arcParts[i];
            var direction = (mainArc.gear.position - center).normalized;
            mainArc.gear.rotation = Quaternion.LookRotation(direction);
        }

        void CreateHalfCircleByAngle(int i)
        {
            var baseAngle = ChainHelper.AngleByDistance(unit, arcParts[i].radius);

            EdgeAngles edgeAngles = arcParts.Length <= 2 ? 
                new EdgeAngles(0, 180) : 
                new EdgeAngles(baseAngle * arcParts[i].edgeSmoother, 180 - baseAngle * arcParts[i].edgeSmoother);

            var mainRadius = arcParts[i].radius;
            // float arcDifferance = mainRadius - arcParts[arcParts[i].relatedArcId].radius;
            // if (arcDifferance > 0 && Mathf.Abs(arcDifferance) > 3)
            // {
            //     edgeAngles.Start = -baseAngle;
            //     edgeAngles.End = 180 + baseAngle;
            // }


            for (float j = edgeAngles.Start; j <= edgeAngles.End; j += baseAngle)
            {
                var newAngle = j;
                arcParts[i].arcPoints.Add(ChainHelper.CirclePoint(newAngle, mainRadius));
            }
        }

        void RotatePoints(int i)
        {
            var arcPoints = arcParts[i].arcPoints;
            var gear = arcParts[i].gear;

            for (var j = 0; j < arcPoints.Count; j++)
            {
                var point = arcPoints[j];
                arcPoints[j] = gear.position + gear.rotation * point;
            }
        }

        void SetLinearPoints()
        {
            SetRelatedArcs();
            for (int i = 0; i < arcParts.Length; i++)
            {
                SetConnectionPoints(i);
                LinearPointAmountByDistance(i);
                AddLinearPoints(i);
            }
        }

        void SetRelatedArcs()
        {
            for (int i = 0; i < arcParts.Length; i++)
            {
                if (i == 0)
                {
                    arcParts[i].relatedArcId = arcParts.Length - 1;
                    continue;
                }

                arcParts[i].relatedArcId = i - 1;
            }
        }

        void SetConnectionPoints(int i)
        {
            var relatedArc = arcParts[arcParts[i].relatedArcId];
            arcParts[i].connectionPoint = relatedArc.arcPoints.First();
        }

        void LinearPointAmountByDistance(int i)
        {
            var distance = Vector3.Distance(
                arcParts[i].arcPoints.Last(),
                arcParts[i].connectionPoint);
            //arcParts[arcParts[i].relatedArcId].arcPoints.Last());

            linearPointAmount = Mathf.RoundToInt(distance / unit) - 1; //TODO: bug: bu da sondaki neyse öyle kalıyordur
        }

        void AddLinearPoints(int i)
        {
            // var relatedArc = arcParts[arcParts[i].relatedArcId];
            Vector3 edgeDirection = (arcParts[i].connectionPoint - arcParts[i].arcPoints.Last()).normalized;
            //(arcParts[i].connectionPoint - arcParts[i].arcPoints.First()).normalized;
            //(relatedArc.arcPoints.Last() - arcParts[i].arcPoints.First()).normalized;//(arcParts[i].arcPoints.Last() - relatedArc.arcPoints.First()).normalized;

            unitDistance = edgeDirection * unit;

            var arcPoints = arcParts[i].arcPoints;
            for (int j = 0; j < linearPointAmount; j++)
            {
                arcPoints.Add(arcPoints.Last() + unitDistance);
            }
        }

        void BindPoints()
        {
            foreach (var arcPart in arcParts)
            {
                chainPoints.AddRange(arcPart.arcPoints);
            }

            chainDrawer.DrawChain(chainPoints);
        }

        void AdaptUnitToCircle()
        {
            unit = Vector3.Distance(chainPoints[0], chainPoints[1]); //print(chainPoints[1].z);
        }
    }

    public class EdgeAngles
    {
        public float Start;
        public float End;
        
        public EdgeAngles(float start, float end)
        {
            Start = start;
            End = end;
        }
    }
}