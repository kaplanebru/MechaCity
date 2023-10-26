using System.Collections.Generic;
using MyNamespace;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class TeethGenerator : MonoBehaviour
    {
        private CogData Data;
        public Transform toothPb;
        
        
        float _intervalAngle = 60;


        private void OnEnable()
        {
            ChainEvents.OnCogDataSet += CreateTeethPoints;
           
        }

        void SetIntervalAngle()
        {
            _intervalAngle = Data.ToothGap / (Data.Radius);
            if (Data.Equalize)
                _intervalAngle = TrigonometryHelper.Angle360(_intervalAngle);

            if (_intervalAngle < Data.MinGapLimit)
                _intervalAngle = Data.MinGapLimit;
        }

        public void CreateTeethPoints(CogData data, Transform teethParent) //CogData data, Transform teethParent //params object[] args
        {
            if (teethParent.position != transform.position) return;
            
            ResetTeeth();
            Data = data;

            Vector3 inverseParentScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y,
                1f / transform.localScale.z);
            SetIntervalAngle();

            for (float i = 0; i < 360; i += _intervalAngle)
            {
                Vector3 point = TrigonometryHelper.CirclePoint(i, Data.Radius);
                //Transform tooth = Instantiate(toothPb, point + transform.position, Quaternion.identity);

                Tooth tooth = ToothPool.Instance.GetItem(t =>
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

            var toothUnit = Vector3.Distance(teeth[1].transform.position, teeth[0].transform.position);//Mathf.Sin(intervalAngle * Mathf.Deg2Rad) * Data.Radius;
            //ChainEvents.OnTeethCreated?.Invoke(teeth.Count, Data.uniqueID, toothUnit);
            SetSpeedByTeeth(teeth.Count, Vector3.Distance(teeth[0].transform.position, teeth[1].transform.position));
        }
        
        
        private void SetSpeedByTeeth(int teethCount, float toothUnit)
        {
            print("Teeth count: " + Data.TeethCount);
            Data.Speed = ChainMover.MachinerySpeed / teethCount;
            print("speed at setter: " + Data.Speed);
            Data.TeethCount = teethCount;
            Data.ToothUnit = toothUnit;
        }

        public List<Tooth> teeth = new();

        void ResetTeeth()
        {
            //if(teeth.Count == 0) return;
            teeth.ForEach(t=>
            {
                t.transform.SetParent(ToothPool.Instance.transform);
                ToothPool.Instance.ReleaseItem(t);
            });
            teeth.Clear();
        }

        private void OnDisable()
        {
            ChainEvents.OnCogDataSet -= CreateTeethPoints;
        }
    }
}