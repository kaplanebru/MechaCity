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

        private void OnEnable()
        {
            ResetValues();
        }

        public void DrawChain(List<Vector3> points)
        {
            _chainPoints = points;
            InstantiateObjs();
            //DrawLines();
        }

        public void InstantiateObjs()
        {
            for (int i = 0; i < _chainPoints.Count; i++)
            {
                var newCube = Instantiate(obj, _chainPoints[i], Quaternion.identity);
                newCube.SetParent(objs);
                if (i == 0)
                    newCube.GetComponent<MeshRenderer>().material = firstCubeMaterial;
            }
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
    }
}

