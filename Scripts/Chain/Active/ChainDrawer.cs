using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;


namespace Chain
{
    public class ChainDrawer : MonoBehaviour
    {
        public ChainData Data;
        public LineRenderer lr;
        Material lrMat;
        public Material firstCubeMaterial;
        public Transform lastLinkPrefab;


        private List<Vector3> _chainPoints = new();
        private List<Transform> _links = new();
        private int _pointsCount;

        private void OnEnable()
        {
            ChainEvents.OnPointsCreated += DrawChain;
            ResetValues();
        }

        void DrawChain(List<Vector3> points)
        {
            _chainPoints = points;
            _pointsCount = _chainPoints.Count;
            if (Data.Type == ChainType.Line)
                DrawLines();
            else
                InstantiateObjs();
        }

        void InstantiateObjs()
        {
            for (int i = 0; i < _pointsCount; i++)
            {
                var link = LinkPool.Instance.GetItem(l => l.transform.position = _chainPoints[i]);

                SetLookRotations(i, link);
                if (Data.Type == ChainType.StandardChain)
                    RotateLinks(i, link);

                _links.Add(link);

                if (i == 0)
                    link.GetComponentInChildren<MeshRenderer>().material = firstCubeMaterial; //debug
            }

            if(Data.Type == ChainType.BikeChain)
                RegulateLastLink();

            ChainEvents.OnLinksCreated?.Invoke(_links);
        }

        void RegulateLastLink()
        {
            if (Vector3.Distance(_chainPoints.First(), _chainPoints.Last()) < Data.Unit)
            {
                var lastLink = _links.Last();
                Vector3 dir = (_links.First().position - lastLink.transform.position).normalized;
                var newLastLink = Instantiate(lastLinkPrefab, lastLink.transform.position + dir , lastLink.transform.rotation);
                _links.Add(newLastLink);
                _chainPoints.Add(newLastLink.transform.position);
            }
        }


        void SetLookRotations(int i, Transform newObj)
        {
            if (i < _pointsCount)
            {
                newObj.transform.rotation =
                    Quaternion.LookRotation(_chainPoints[(i + 1) % _pointsCount] - _chainPoints[i]);
            }
        }

        void RotateLinks(int i, Transform newObj)
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
            _links.ForEach(l => LinkPool.Instance.ReleaseItem(l));
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