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
        [SerializeField] private ChainType chainType;
        public static ChainType ChainType;

        public bool isMoving = true;
        public Arc[] arcs;

        public float unit = 2.2f;
        private int linearPointAmount;
        private Vector3 unitDistance;
        private Vector3 _center;

        [ReadOnly] public List<Vector3> chainPoints = new();

        private void Start()
        {
            chainPoints.Clear();
            Setup();

            SetCircularPoints();
            SetLinearPoints();
            BindPoints();
        }

        void Setup()
        {
            ChainType = chainType;
            ChainEvents.OnMotionDecision?.Invoke(isMoving);
            _center = ChainHelper.CenterDirection(arcs);
            SetIDs();
            RelateArcs();
        }

        void SetIDs()
        {
            for (int i = 0; i < arcs.Length; i++)
            {
                arcs[i].id = i;
            }
        }

        void RelateArcs()
        {
            for (int i = 0; i < arcs.Length; i++)
            {
                var id = i - 1;
                if (id < 0) id += arcs.Length;
                arcs[i].relatedArcId = id;
            }
        }

        void SetCircularPoints()
        {
            SetArcRotation(0);
            CreateHalfCircleByAngle(0);
            RotatePoints(0);
        }

        void SetArcRotation(int i)
        {
            var mainArc = arcs[i];
            var direction = (mainArc.gear.position - _center).normalized;
            mainArc.gear.rotation = Quaternion.LookRotation(direction);

            if (mainArc.relatedArcId == 0) return;
            SetArcRotation(mainArc.relatedArcId);
        }

        void CreateHalfCircleByAngle(int i)
        {
            var baseAngle = ChainHelper.AngleByDistance(unit, arcs[i].radius);

            EdgeAngles edgeAngles = arcs.Length <= 2
                ? new EdgeAngles(0, 180)
                : new EdgeAngles(baseAngle * arcs[i].edgeSmoother, 180 - baseAngle * arcs[i].edgeSmoother);

            var mainRadius = arcs[i].radius;
            // float arcDifferance = mainRadius - arcParts[arcParts[i].relatedArcId].radius;
            // if (arcDifferance > 0 && Mathf.Abs(arcDifferance) > 3)
            // {
            //     edgeAngles.Start = -baseAngle;
            //     edgeAngles.End = 180 + baseAngle;
            // }


            for (float j = edgeAngles.Start; j <= edgeAngles.End; j += baseAngle)
            {
                var newAngle = j;
                arcs[i].arcPoints.Add(ChainHelper.CirclePoint(newAngle, mainRadius));
            }

            if (arcs[i].relatedArcId == 0) return;
            CreateHalfCircleByAngle(arcs[i].relatedArcId);
        }

        void RotatePoints(int i)
        {
            var arcPoints = arcs[i].arcPoints;
            var gear = arcs[i].gear;

            for (var j = 0; j < arcPoints.Count; j++)
            {
                var point = arcPoints[j];
                arcPoints[j] = gear.position + gear.rotation * point;
            }

            if (arcs[i].relatedArcId == 0) return;
            RotatePoints(arcs[i].relatedArcId);
        }

        void SetLinearPoints()
        {
            SetConnectionPoints(0);
            AddLinearPoints(0);
        }


        void SetConnectionPoints(int i)
        {
            var relatedArc = arcs[arcs[i].relatedArcId];
            arcs[i].connectionPoint = relatedArc.arcPoints.First(); //bug: hiç point yoksa geliyor

            if (relatedArc.id == 0) return;
            SetConnectionPoints(relatedArc.id);
        }


        void AddLinearPoints(int i)
        {
            linearPointAmount =
                ChainHelper.LinearPointAmountByDistance(arcs[i].connectionPoint, arcs[i].arcPoints.Last(), unit);
            Vector3 edgeDirection = (arcs[i].connectionPoint - arcs[i].arcPoints.Last()).normalized;
            //(arcParts[i].connectionPoint - arcParts[i].arcPoints.First()).normalized;

            unitDistance = edgeDirection * unit;

            var arcPoints = arcs[i].arcPoints;
            for (int j = 0; j < linearPointAmount; j++)
            {
                arcPoints.Add(arcPoints.Last() + unitDistance);
            }

            if (arcs[i].relatedArcId == 0) return;
            AddLinearPoints(arcs[i].relatedArcId);
        }

        void BindPoints()
        {
            int i = 0;
            while (true)
            {
                chainPoints.AddRange(arcs[i].arcPoints);
                i = arcs[i].relatedArcId;
                if (i == 0) break;
            }

            ChainEvents.OnPointsCreated?.Invoke(chainPoints);
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