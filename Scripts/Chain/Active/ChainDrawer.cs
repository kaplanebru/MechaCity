using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Chain
{
    public class ChainDrawer : MonoBehaviour
    {
        public LineRenderer lr;
        Material lrMat;
        public Material firstCubeMaterial;
        public Transform obj;
        public Transform objs;

        private List<Vector3> _chainPoints = new();
        private List<Transform> _chains = new();

        private void OnEnable()
        {
            ChainEvents.OnPointsCreated += DrawChain;
            ResetValues();
        }

        void DrawChain(List<Vector3> points)
        {
            _chainPoints = points;
            _pointsCount = _chainPoints.Count;
            InstantiateObjs();
            //DrawLines();
        }

        private int _pointsCount;

        void InstantiateObjs()
        {
            for (int i = 0; i < _pointsCount; i++)
            {
                var newObj = Instantiate(obj, _chainPoints[i], Quaternion.identity);

                RotateChains(i, newObj);
                BindChains(i, newObj);
                newObj.SetParent(objs);
                _chains.Add(newObj);
                
                if (i == 0)
                    newObj.GetComponentInChildren<MeshRenderer>().material = firstCubeMaterial; //Temp
            }

            ChainEvents.OnObjsCreated?.Invoke(_chains);
        }


        void RotateChains(int i, Transform newObj)
        {
            if (i < _pointsCount)
                newObj.transform.rotation =
                    Quaternion.LookRotation(_chainPoints[(i + 1) % _pointsCount] - _chainPoints[i]);
        }

        void BindChains(int i, Transform newObj)
        {
            var rot = newObj.transform.rotation;
            if (i % 2 == 0)
                newObj.transform.rotation =
                    Quaternion.Euler(rot.eulerAngles.x,
                        rot.eulerAngles.y,
                        rot.eulerAngles.z - 90);
        }

        void DrawLines()
        {
            lr.positionCount = _chainPoints.Count;
            lr.SetPositions(_chainPoints.ToArray());
        }
        
        void ResetValues()
        {
            _chainPoints.Clear();
            lr.positionCount = 0;
        }

        private void OnDisable()
        {
            ChainEvents.OnPointsCreated -= DrawChain;
        }
    }
}