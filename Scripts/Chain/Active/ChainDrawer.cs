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
        

        private List<Vector3> _chainPoints = new();
        private List<Transform> _links = new();

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
                var link = LinkPool.Instance.GetItem(l => l.transform.position = _chainPoints[i]);
                //var link = Instantiate(linkPrefab, _chainPoints[i], Quaternion.identity);

                RotateChains(i, link);
                BindChains(i, link);
               // link.SetParent(linksHolder);
                _links.Add(link);
                
                if (i == 0)
                    link.GetComponentInChildren<MeshRenderer>().material = firstCubeMaterial; //Temp
            }

            ChainEvents.OnLinksCreated?.Invoke(_links);
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

        public void ReleaseChain()
        {
            _links.ForEach(l=>LinkPool.Instance.ReleaseAndDeactivateItem(l));
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