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
        public ChainMover chainMover;

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
                var newObj = Instantiate(obj, _chainPoints[i], Quaternion.identity);
                if(i < _chainPoints.Count-1)
                    newObj.transform.rotation = Quaternion.LookRotation(_chainPoints[i + 1] - _chainPoints[i]);
                var rot = newObj.transform.rotation;
                if (i % 2 == 0)
                    newObj.transform.rotation =
                        Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y, rot.eulerAngles.z - 90);
                                                 
                newObj.SetParent(objs);
                if (i == 0)
                    newObj.GetComponentInChildren<MeshRenderer>().material = firstCubeMaterial;
                
                _chains.Add(newObj);
            }
            chainMover.Setup(_chains);
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

