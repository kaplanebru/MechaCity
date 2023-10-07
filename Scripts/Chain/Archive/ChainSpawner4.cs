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
    public class ChainSpawner4 : MonoBehaviour
    {
        public LineRenderer lr;
        Material lrMat;
        public Material firstCubeMaterial;
        
        public float radius = 1;
        public float unit = 1;

        public Transform sphere;
        public Transform spheres;
        
        public Transform center;
        public Transform destination;

        public int linearPointAmount = 1;
        
        private int totalAmount;
        private float baseAngle;
        private Vector3 unitDistance;
        [ReadOnly] public List<Vector3> chainPoints = new();


        private void Start()
        {
            ResetValues();
            
            AngleByDistance();
            CirclePointsByAngle();
            
            AdaptUnitToCircle();
            LinearPointAmountByDistance();
            
            SplitCircle();
            InsertLinearPoints();
            
            RotatePoints();
            
            InstaintiateCubes();
            DrawLines();
            print(totalAmount);
        }

        void RotatePoints()
        {
            for (var i = 0; i < chainPoints.Count; i++)
            {
                var point = chainPoints[i];
                chainPoints[i] = spheres.position + spheres.rotation * point;
            }
        }

        void LinearPointAmountByDistance()
        {
            var distance = Vector3.Distance(center.position, destination.position);
            linearPointAmount = Mathf.RoundToInt(distance / unit) - 1;
        }
        
        Vector3 CirclePoint(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);

            return new Vector3(x, 0, y) * radius; // + transform.position;
        }

        void AngleByDistance()
        {
            baseAngle = Mathf.Asin(unit / radius) * Mathf.Rad2Deg;

            var intAngle = Mathf.RoundToInt(baseAngle);
            int rest = intAngle % 6;
            baseAngle = rest / 2 < 2 ? intAngle - rest : intAngle + 6 - rest;
        }

        void CirclePointsByAngle()
        {
            for (float i = 0; i < 360; i += baseAngle)
            {
                totalAmount++;
                var newAngle = i;
                chainPoints.Add(CirclePoint(newAngle));
            }
        }
        
        void AdaptUnitToCircle()
        {
            unit = Vector3.Distance(chainPoints[0], chainPoints[1]); //print(chainPoints[1].z);
        }

        void InsertIntersectionPoints()
        {
            chainPoints.Insert(totalAmount / 2 , chainPoints[totalAmount / 2 ]);
            totalAmount+=2;
            chainPoints.Add(chainPoints[0]);
        }
        void SplitCircle()
        {
            InsertIntersectionPoints();
            for (int i = totalAmount / 2; i < totalAmount; i++)
            {
                var pos = chainPoints[i];
                pos.z -= (linearPointAmount + 1) * unit;
                chainPoints[i] = pos;
            }
        }
        void InsertLinearPoints()
        {
            unitDistance = Vector3.forward * unit;
            
            int start = totalAmount / 2;
            int end = start + linearPointAmount;
            int multiplier = 0;

            var lastPoint = chainPoints[start - 1];
            for (int i = start; i < end; i++)
            {
                multiplier++;
                chainPoints.Insert(i, lastPoint - unitDistance * multiplier);
            }
            
            for (int i = 0; i < linearPointAmount; i++)
            {
                chainPoints.Add(chainPoints.Last() + unitDistance);
            }
            
            totalAmount += linearPointAmount * 2;
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