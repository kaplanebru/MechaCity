using System.Collections.Generic;
using MyNamespace;
using UnityEngine;


namespace Chain
{
    public class TeethGenerator : MonoBehaviour
    {
        private CogData Data;
        public Transform toothPb;
        public float intervalAngle = 60;
        public float minIntervalLimit = 6;
        public bool equalize = false;

        private void OnEnable()
        {
            ChainEvents.OnCogDataSet += CreateTeethPoints;
        }

        void SetIntervalAngle()
        {
            intervalAngle /= (Data.Radius);

            if (equalize)
                intervalAngle = TrigonometryHelper.Angle360(intervalAngle);

            if (intervalAngle < minIntervalLimit)
                intervalAngle = minIntervalLimit;
        }

        void CreateTeethPoints(CogData data, Transform teethParent) //CogData data, Transform teethParent //params object[] args
        {
            if (teethParent.position != transform.position) return;
            Data = data;

            Vector3 inverseParentScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y,
                1f / transform.localScale.z);
            SetIntervalAngle();

          
            
            for (float i = 0; i < 360; i += intervalAngle)
            {
                Vector3 point = TrigonometryHelper.CirclePoint(i, Data.Radius);
                //Transform tooth = Instantiate(toothPb, point + transform.position, Quaternion.identity);

                var tooth = ToothPool.Instance.GetItem(t =>
                {
                    t.transform.position = transform.position + transform.rotation * point;
                    t.transform.SetParent(teethParent);

                    t.transform.localScale = Vector3.Scale(Data.toothScale, inverseParentScale);


                    t.transform.localRotation = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                        ? Quaternion.LookRotation(point)
                        : Quaternion.LookRotation(point, Vector3.forward);
                });

              
                teeth.Add(tooth);
                //TODO: follow olmayan koşlda takip etmesin
            }

            ChainEvents.OnTeethCreated?.Invoke(teeth.Count, transform, Mathf.Sin(intervalAngle * Mathf.Deg2Rad) * Data.Radius);
        }

        public List<Tooth> teeth = new();

        private void OnDisable()
        {
            print(teeth.Count);
            teeth.ForEach(t=>ToothPool.Instance.ReleaseItem(t));
            ChainEvents.OnCogDataSet -= CreateTeethPoints;
        }
    }
}