using UnityEngine;

namespace Curves
{
    public class LineCreator
    {
        private LineRenderer[] LineRenderers;

        public LineCreator(params LineRenderer[] lineRenderers)
        {
            LineRenderers = lineRenderers;
            Debug.Log( "lr: "+LineRenderers.Length);
        }

        public void EnableLines(int lineAmount)
        {
            for (int i = 0; i < lineAmount; i++)
            {
                LineRenderers[i].enabled = true;
            }
        }

        public void DisableLines()
        {
            foreach (var lr in LineRenderers)
            {
                lr.enabled = false;
            }
        }
        
        public void PointsToLines(int index, Vector3[] points)
        {
            var lr = LineRenderers[index];
            lr.enabled = true;
            lr.positionCount = 0;
            lr.positionCount = points.Length;
            lr.SetPositions(points);
        }
        
    }
}