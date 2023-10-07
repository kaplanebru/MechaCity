using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;

//DISTANCE BASED
namespace Chain
{
    [Serializable]
    public class ArcPart
    {
        [ReadOnly] public int id;
        public float radius;
        public Transform gear;
        public int relatedArcId;

        public List<Vector3> arcPoints = new();
    }

    public class ChainSpawner2 : MonoBehaviour
    {
        public ArcPart[] arcParts;
        public LineRenderer lr;
        Material lrMat;
        public Material firstCubeMaterial;

        public float unit = 1;
        public int linearPointAmount = 1;

        public Transform sphere;
        public Transform objs;


        private Vector3 unitDistance;
        [ReadOnly] public List<Vector3> chainPoints = new();

        private void Start()
        {
            ResetValues();
            SetCircularPoints();
            SetLinearPoints();


            BindPoints();
            InstantiateObjs();
            DrawLines();
        }

        void BindPoints()
        {
            foreach (var arcPart in arcParts)
            {
                chainPoints.AddRange(arcPart.arcPoints);
            }
        }

        void SetCircularPoints()
        {
            for (int i = 0; i < arcParts.Length; i++)
            {
                SetArcRotation(i);
                CreateHalfCircleByAngle(i);
                RotatePoints(i);
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


        void SetArcRotation(int i)
        {
            var mainArc = arcParts[i];
            var relatedArc = arcParts[mainArc.relatedArcId];
            var direction = (mainArc.gear.position - relatedArc.gear.position).normalized;

            mainArc.gear.rotation = Quaternion.LookRotation(direction);
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

        void LinearPointAmountByDistance(int i)
        {
            var distance = Vector3.Distance(
                arcParts[i].arcPoints.First(),
                arcParts[arcParts[i].relatedArcId].arcPoints.Last());

            linearPointAmount = Mathf.RoundToInt(distance / unit) - 1;
        }

        Vector3 CirclePoint(float angle, float radius)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);

            return new Vector3(x, 0, y) * radius;
        }

        int AngleByDistance(int i)
        {
            var baseAngle = Mathf.Asin(unit / arcParts[i].radius) * Mathf.Rad2Deg;

            var intAngle = Mathf.RoundToInt(baseAngle);
            int rest = intAngle % 6;
            return rest / 2 < 2 ? intAngle - rest : intAngle + 6 - rest;
            //return intAngle;
        }

        void CreateHalfCircleByAngle(int i)
        {
            var baseAngle = AngleByDistance(i);
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
                arcParts[i].arcPoints.Add(CirclePoint(newAngle, mainRadius));
            }
        }

        void AdaptUnitToCircle()
        {
            unit = Vector3.Distance(chainPoints[0], chainPoints[1]); //print(chainPoints[1].z);
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


        void InstantiateObjs()
        {
            for (int i = 0; i < chainPoints.Count; i++)
            {
                var newCube = Instantiate(sphere, chainPoints[i], Quaternion.identity);
                newCube.SetParent(objs);
                if (i == 0)
                    newCube.GetComponent<MeshRenderer>().material = firstCubeMaterial;
            }
        }

        void DrawLines()
        {
            chainPoints.Add(chainPoints[0]);
            lr.positionCount = chainPoints.Count;
            lr.SetPositions(chainPoints.ToArray());
        }


        void ResetValues()
        {
            chainPoints.Clear();
            lr.positionCount = 0;
        }
    }
}