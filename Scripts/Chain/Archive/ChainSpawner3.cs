using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;


namespace Chain
{
    public class ChainSpawner3 : MonoBehaviour
    {
        public LineRenderer lr;
        Material lrMat;
        public float radius = 1;

        public Material firstCubeMaterial;
        public Material linearPointMaterial;
        private float directionAngle;
        private Vector3 direction;


        public int circleAmount = 6;

        private int CircleAmount
        {
            get => circleAmount - circleAmount % 6;
            set {}
        }

        private int totalAmount;

        public Transform sphere;
        public Transform center;
        public Transform destination;

        [ReadOnly] public List<Vector3> chainPoints = new();


        private void Start()
        {
            ResetValues();

            GetCirclePoints();
            GetDistanceBetweenPoints();
            SplitCircle();
            InsertLinearPoints();
            InstaintiateCubes();
            DrawLines();
        }
        
        void InstaintiateCubes()
        {
            for (int i = 0; i < totalAmount; i++)
            {
                var newCube = Instantiate(sphere, chainPoints[i], Quaternion.identity);
                if (i == 0)
                    newCube.GetComponent<MeshRenderer>().material = firstCubeMaterial;

            }
        }
        void GetCirclePoints()
        {
            //SetDirectionAngle();
            float baseAngle = 360f / CircleAmount;

            
            for (int i = 0; i <= CircleAmount; i++)
            {
                var newAngle = (baseAngle * i + directionAngle) % 360; //print(newAngle);
                chainPoints.Add(CirclePoint(newAngle));
            }
            
            InsertIntersectionPoints();
        }

        void InsertIntersectionPoints()
        {
            chainPoints.Insert(chainPoints.Count/2, chainPoints[chainPoints.Count/2]);
            chainPoints.Insert(chainPoints.Count-1, chainPoints[0]);
            totalAmount = CircleAmount + 2;
        }
        
        Vector3 CirclePoint(float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(radians);
            float y = Mathf.Sin(radians);

            return new Vector3(x, 0, y) * radius; // + transform.position;
        }

        private Vector3 pointDistance;
        public int linearPointAmount = 1;
        void GetDistanceBetweenPoints()
        {
            pointDistance = Vector3.Distance(chainPoints[0], chainPoints[1]) * Vector3.forward;
            print(pointDistance.z);
        }


        void SplitCircle()
        {
            for (int i = totalAmount / 2; i < totalAmount; i++)
            {
                var pos = chainPoints[i];
                pos.z -= (linearPointAmount+1) * pointDistance.z;
                chainPoints[i] = pos;
            }
        }

        void InsertLinearPoints()
        {
            int start = totalAmount / 2;
            int end = start + linearPointAmount;
            int multiplier = 0;
            
            var lastPoint = chainPoints[start-1];
            for (int i = start; i < end; i++)
            {
                multiplier++;
                
                
                chainPoints.Insert(i, lastPoint - pointDistance * multiplier);
            }

            totalAmount += linearPointAmount;
            

            start = chainPoints.Count-1;
            end = start + linearPointAmount;
            multiplier = 0;
            lastPoint = chainPoints[start - 1];
            print(lastPoint.z);

            for (int i = start; i < end; i++)
            {
                multiplier++;
                chainPoints.Insert(i, lastPoint + pointDistance * multiplier);
            }

            totalAmount += linearPointAmount;
        }
        

        void AddBindingPoint()
        {
            chainPoints.Add(chainPoints[0]);
            totalAmount++;
        }

        void DrawLines()
        {
            AddBindingPoint();
            lr.positionCount = totalAmount;//CircleAmount + 1;
            lr.SetPositions(chainPoints.ToArray());
        }

        
        void ResetValues()
        {
            chainPoints.Clear();
            lr.positionCount = 0;
        }

        void SetDirectionAngle()
        {
            direction = (destination.transform.position - transform.position).normalized;
            directionAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        }
    

        
    }
}