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
    public class ArcParts
    {
        public List<Vector3> arcPoints = new();
        public bool smaller;
        public bool first;
    }

    public class ChainSpawner2 : MonoBehaviour
    {
        public LineRenderer lr;
        Material lrMat;
        public Material firstCubeMaterial;

        public float[] lesRadius;
        public float unit = 1;

        public Transform sphere;
        public Transform[] arcs;
        public Transform spheres;


        public int linearPointAmount = 1;

        private int totalAmount;

        private Vector3 unitDistance;
        [ReadOnly] public List<Vector3> chainPoints = new();


        public ArcParts[] arcParts;
        private Vector3 _direction;


        private void OnEnable()
        {
            arcParts = new ArcParts[arcs.Length];
            for (int i = 0; i < arcs.Length; i++)
            {
                arcParts[i] = new ArcParts();
            }
        }

        private void Start()
        {
            SetArcRotation(0);
            ResetValues();
            //LinearPointAmountByDistance();

            for (int i = 0; i < arcs.Length; i++)
            {
                CreateHalfCircleByAngle(i);
                RotatePoints(i);
            }

            LinearPointAmountByDistance();
            for (int i = 0; i < arcs.Length; i++)
            {
                AddLinearPoints(i);
            }


            for (int i = 0; i < arcParts.Length; i++)
            {
                chainPoints.AddRange(arcParts[i].arcPoints);
            }

            InstaintiateCubes();
            DrawLines();
        }

        void SetArcRotation(int firstArc)
        {
            _direction = (arcs[1].position - arcs[0].position).normalized;
            arcs[firstArc].rotation = Quaternion.LookRotation(-_direction);
            arcs[firstArc + 1].rotation = Quaternion.LookRotation(_direction);
        }

        void RotatePoints(int arcIndex)
        {
            var arcPoints = arcParts[arcIndex].arcPoints;
            for (var i = 0; i < arcPoints.Count; i++)
            {
                var point = arcPoints[i];
                arcPoints[i] = arcs[arcIndex].position + arcs[arcIndex].rotation * point;
            }
        }

        void LinearPointAmountByDistance()
        {
            var distance = Vector3.Distance(arcParts[1].arcPoints.Last(), arcParts[0].arcPoints.First());
            linearPointAmount = Mathf.RoundToInt(distance / unit) - 1;
        }

        Vector3 CirclePoint(float angle, int arcIndex)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);

            return new Vector3(x, 0, y) * lesRadius[arcIndex]; // + transform.position;
        }

        int AngleByDistance(int arcIndex)
        {
            var baseAngle = Mathf.Asin(unit / lesRadius[arcIndex]) * Mathf.Rad2Deg;

            var intAngle = Mathf.RoundToInt(baseAngle);
            int rest = intAngle % 6;
            return rest / 2 < 2 ? intAngle - rest : intAngle + 6 - rest;
            //return intAngle;
        }

        void CreateHalfCircleByAngle(int arcIndex)
        {
            var baseAngle = AngleByDistance(arcIndex);
            int start, max;
            start = 0;
            max = 180;
            
            
            if(Mathf.Abs(lesRadius[0]-lesRadius[1]) > 3 && arcIndex == 1)
            {
                start = -baseAngle;
                max = 180 + baseAngle;
            }

            for (float i = start; i <= max; i += baseAngle)
            {
                totalAmount++;
                var newAngle = i;
                arcParts[arcIndex].arcPoints.Add(CirclePoint(newAngle, arcIndex));
            }
        }

        void AdaptUnitToCircle()
        {
            unit = Vector3.Distance(chainPoints[0], chainPoints[1]); //print(chainPoints[1].z);
        }


        Vector3 _edgeDirection;

        void AddLinearPoints(int arcIndex)
        {
            if (arcIndex == 0)
                _edgeDirection = (arcParts[0].arcPoints.Last() - arcParts[1].arcPoints.First()).normalized;
            else
                _edgeDirection = (arcParts[1].arcPoints.Last() - arcParts[0].arcPoints.First()).normalized;


            unitDistance = _edgeDirection * unit;


            var arcPoints = arcParts[arcIndex].arcPoints;

            for (int i = 0; i < linearPointAmount; i++)
            {
                arcPoints.Add(arcPoints.Last() - unitDistance);
            }

            totalAmount += linearPointAmount;
        }


        void InstaintiateCubes()
        {
            for (int i = 0; i < totalAmount; i++)
            {
                var newCube = Instantiate(sphere, chainPoints[i], Quaternion.identity);
                newCube.SetParent(spheres);
                if (i == 0)
                    newCube.GetComponent<MeshRenderer>().material = firstCubeMaterial;
            }
        }

        void AddBindingPoint()
        {
            chainPoints.Add(chainPoints[0]);
            totalAmount++;
        }

        void DrawLines()
        {
            AddBindingPoint();
            lr.positionCount = totalAmount; //CircleAmount + 1;
            lr.SetPositions(chainPoints.ToArray());
        }


        void ResetValues()
        {
            chainPoints.Clear();
            lr.positionCount = 0;
        }
    }
}