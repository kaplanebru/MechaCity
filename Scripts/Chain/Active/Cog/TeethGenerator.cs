using UnityEngine;


namespace Chain
{
    public class TeethGenerator : MonoBehaviour
    {
        public CogData Data;
        public Transform toothPb;
        public float intervalAngle = 60;
        public float minIntervalLimit = 6;
        public bool equalize = false;

        private int teethCount;

       

        private void OnEnable()
        {
            ChainEvents.OnCogStart += CreateTeethPoints;
        }

        void SetIntervalAngle()
        {
            intervalAngle /= (Data.Radius);

            if (equalize)
                intervalAngle = TrigonometryHelper.Angle360(intervalAngle);

            if (intervalAngle < minIntervalLimit)
                intervalAngle = minIntervalLimit;
        }

        void CreateTeethPoints(CogData data, Transform parent)
        {
            if (parent.position != transform.position) return;
            Data = data;


            SetIntervalAngle();
            for (float i = 0; i < 360; i += intervalAngle)
            {
                teethCount++;
                Vector3 point = TrigonometryHelper.CirclePoint(i, Data.Radius);
                //Transform tooth = Instantiate(toothPb, point + transform.position, Quaternion.identity);
                Tooth tooth = ToothPool.Instance.GetItem(t => t.transform.position = point + transform.position);
                
                SetTooth(tooth, point);
                tooth.transform.SetParent(parent);
            }

            ChainEvents.OnTeethCreated?.Invoke(teethCount, transform, Mathf.Sin(intervalAngle * Mathf.Deg2Rad) * Data.Radius);
        }

        void SetTooth(Tooth tooth, Vector3 direction)
        {
            tooth.transform.rotation = Quaternion.LookRotation(direction);
            tooth.transform.localScale = Data.toothScale;

            // Vector3 scale = tooth.transform.localScale;
            // scale.x *= Data.toothScale.x;
            // scale.z *= Data.toothScale.z;
            // tooth.transform.localScale = scale;
        }

        private void OnDisable()
        {
            ChainEvents.OnCogStart -= CreateTeethPoints;
        }
    }
}