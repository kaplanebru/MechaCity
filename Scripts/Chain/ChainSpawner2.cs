using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Collections;
using UnityEngine;


namespace Chain
{
    public class ChainSpawner2 : MonoBehaviour
    {
        public LineRenderer lr;
        Material lrMat;
        public float radius = 1;

        public Material firstCubeMaterial;
        private float directionAngle;
        private Vector3 direction;


        public int circleAmount;

        private int CircleAmount
        {
            get => circleAmount - circleAmount % 6;
            set {}
        }

        private int totalAmount;

        public Transform cube;
        public Transform center;
        public Transform destination;

        [ReadOnly] public List<Vector3> chainPoints = new();


        private void Start()
        {
            ResetValues();

            GetCirclePoints();
            SplitCircle();
            InstaintiateCubes();
            //DrawLines();
        }
        
        void InstaintiateCubes()
        {
            for (int i = 0; i < totalAmount; i++)
            {
                var newCube = Instantiate(cube, chainPoints[i], Quaternion.identity);
                if (i == 0)
                    newCube.GetComponent<MeshRenderer>().material = firstCubeMaterial;

            }
        }
        void GetCirclePoints()
        {
            SetDirectionAngle();
            float baseAngle = 360f / CircleAmount;

            directionAngle = 0;
            for (int i = 0; i <= CircleAmount; i++)
            {
                var newAngle = (baseAngle * i + directionAngle) % 360; //print(newAngle);
                chainPoints.Add(CirclePoint(newAngle));
            }
            
            InsertIntersectionPoints();
            
            //chainPoints.Add(chainPoints[0]);
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

        void SplitCircle()
        {
            for (int i = totalAmount / 2; i < totalAmount; i++) //+1
            {
                var pos = chainPoints[i];
                pos.z -= 10;
                //pos.z += destination.transform.position.z;
                //pos.x += destination.transform.position.x;
                //pos += direction;
                chainPoints[i] = pos;
            }
        }

        void DrawLines()
        {
            lr.positionCount = CircleAmount + 1;
            lr.SetPositions(chainPoints.ToArray());
        }

    

        
    }
}