using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum LineType
{
    Straight,
    Curved
}

namespace Chain
{
    public class ChainSpawner : MonoBehaviour
    {
        public LineRenderer lr;
        Material lrMat;

        public int curveAmount = 15;


        public Transform[] curveEdges;
        public Transform[] lineEdge;

        [ReadOnly] public List<Vector3> chainPoints = new();


        private void Start()
        {
            chainPoints.Clear();
            lr.positionCount = 0;
            GeneratePoints(curveAmount);

            DrawChain();
            CloseEdges();

            StartCoroutine(nameof(MoveRoutine));
        }

        IEnumerator MoveRoutine()
        {
            lrMat = lr.material;
            for (int i = 0; i < 100; i++)
            {
                var tex = lrMat.mainTextureOffset;
                tex.x = i / 50f;
                lrMat.mainTextureOffset = tex;
                print("y");
                yield return new WaitForSeconds(0.1f);
            }
        }

        void DrawChain()
        {
            lr.positionCount = curveAmount * 3 - offset * 2;
            lr.SetPositions(chainPoints.ToArray());
        }

        Vector3 CurvePoint(float t)
        {
            Vector3 AB = Vector3.Lerp(curveEdges[0].position, curveEdges[1].position, t);
            Vector3 BC = Vector3.Lerp(curveEdges[1].position, curveEdges[2].position, t);
            return Vector3.Lerp(AB, BC, t);
        }

        Vector3 StraightPoint(float t, bool first)
        {
            if (first)
                return Vector3.Lerp(lineEdge[0].position, curveEdges[0].position, t);
            else
                return Vector3.Lerp(curveEdges[2].position, lineEdge[1].position, t);
        }

        public int offset = 1;

        void GeneratePoints(int _amount)
        {
            float ratio = 1f / _amount;
            float t = 0;


            int counter = 0;
            while (t < 1) //line'ın sonu alınmadı
            {
                counter++;

                t = Mathf.MoveTowards(t, 1, ratio);
                chainPoints.Add(StraightPoint(t, true));
                if (counter == _amount - offset) break;
            }

            counter = 0;
            t = 0;
            while (t < 1)
            {
                counter++;
                t = Mathf.MoveTowards(t, 1, ratio);

                // if (counter < 5)
                // {
                //     counter++;
                //     continue;
                // }
                chainPoints.Add(CurvePoint(t));
                if (counter == _amount - offset) break;
                // counter++;
            }

            t = 0;
            while (t < 1)
            {
                t = Mathf.MoveTowards(t, 1, ratio);
                chainPoints.Add(StraightPoint(t, false));
            }
        }

        void CloseEdges()
        {
            foreach (var curveEdge in curveEdges)
            {
                curveEdge.gameObject.SetActive(false);
            }
        }
    }
}