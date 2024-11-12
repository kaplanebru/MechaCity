using UnityEngine;

namespace Curves
{
    public class LineCreator
    {
        private LineRenderer[] LineRenderers;
        public LineCreator(params LineRenderer[] lineRenderers)
        {
            LineRenderers = lineRenderers;
        }
        public void PointsToLines(int index, Vector3[] points)
        {
            var lr = LineRenderers[index];
            lr.enabled = true;
            lr.positionCount = 0;
            lr.positionCount = points.Length;
            lr.SetPositions(points);
        }
        public void DisableLines()
        {
            foreach (var lr in LineRenderers)
            {
                lr.enabled = false;
            }
        }
    }
}