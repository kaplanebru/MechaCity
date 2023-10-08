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

            if (state == ChainState.Odd)
            {
                SetCircularPoints2();
               
                BindPoints();
                chainDrawer.InstantiateObjs();

            }
            else if(state == ChainState.Even)
            {
                SetCircularPoints();
                SetLinearPoints();
                BindPoints();
            }
           
        }

        private Vector3 center;

        void GetCenter()
        {
            center = ChainHelper.CenterDirection(arcParts);
        }
        void SetCircularPoints2()
        {
            GetCenter();

            for (var i = 0; i < arcParts.Length; i++)
            {
                var arcPart = arcParts[i];
                var direction = (arcPart.gear.position - center).normalized;
                arcPart.gear.rotation = Quaternion.LookRotation(direction);
                CreateHalfCircleByAngle(i);
                RotatePoints(i);
            }
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
            
            // var relatedArc = arcParts[mainArc.relatedArcId];
            // var direction = (mainArc.gear.position - relatedArc.gear.position).normalized;
            
            var direction = (mainArc.gear.position - center).normalized;
            mainArc.gear.rotation = Quaternion.LookRotation(direction);
        }
        
        void CreateHalfCircleByAngle(int i)
        {
            var baseAngle = ChainHelper.AngleByDistance(unit, arcParts[i].radius);
            int start, max;

            var mainRadius = arcParts[i].radius;
            float arcDifferance = mainRadius - arcParts[arcParts[i].relatedArcId].radius;
            if (arcDifferance > 0 && Mathf.Abs(arcDifferance) > 3)
            {
                start = -baseAngle;
                max = 180 + baseAngle;
            }
            else
            {
                start = 0;
                max = 180;
            }

            for (float j = start; j <= max; j += baseAngle)
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
            for (int i = 0; i < arcParts.Length; i++)
            {
                LinearPointAmountByDistance(i);
            }
            for (int i = 0; i < arcParts.Length; i++)
            {
                AddLinearPoints(i);
            }
        }

        void LinearPointAmountByDistance(int i)
        {
            var distance = Vector3.Distance(
                arcParts[i].arcPoints.First(),
                arcParts[arcParts[i].relatedArcId].arcPoints.Last());

            linearPointAmount = Mathf.RoundToInt(distance / unit) - 1;
        }
        void AddLinearPoints(int i)
        {
            var relatedArc = arcParts[arcParts[i].relatedArcId];
            Vector3 edgeDirection = (arcParts[i].arcPoints.Last() - relatedArc.arcPoints.First()).normalized;

            unitDistance = edgeDirection * unit;

            var arcPoints = arcParts[i].arcPoints;
            for (int j = 0; j < linearPointAmount; j++)
            {
                arcPoints.Add(arcPoints.Last() - unitDistance);
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
}