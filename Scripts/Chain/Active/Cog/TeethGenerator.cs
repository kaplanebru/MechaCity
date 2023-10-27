using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyNamespace;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class TeethGenerator : MonoBehaviour
    {
        private CogData Data;
        public Transform toothPb;
        private Transform _teethParent;
        
        
        float _intervalAngle = 60;


        private void OnEnable()
        {
            ChainEvents.OnCogDataSet += ReadyForTeethCreation;
            ChainEvents.OnPoolCreated += WaitForPool;
        }

        void ReadyForTeethCreation(CogData data, Transform teethParent)
        {
            if (teethParent.position != transform.position) return;
            ;
            Data = data;
            _teethParent = teethParent;
            StartCoroutine(nameof(CreateTeethRoutine));
        }

        private bool waitPool = true;
        void WaitForPool()
        {
            print("wait false");
            waitPool = false;
        }

        void SetIntervalAngle()
        {
            _intervalAngle = Data.ToothGap / (Data.Radius);
            if (Data.Equalize)
                _intervalAngle = TrigonometryHelper.Angle360(_intervalAngle);

            if (_intervalAngle < Data.MinGapLimit)
                _intervalAngle = Data.MinGapLimit;
        }

        IEnumerator CreateTeethRoutine()
        {
            yield return new WaitUntil(() => waitPool == false);
            CreateTeethPoints();
        }

        public void CreateTeethPoints() //CogData data, Transform teethParent //params object[] args
        {
            ResetTeeth();

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
                    t.transform.SetParent(_teethParent);

                    t.transform.localScale = Vector3.Scale(Data.toothScale, inverseParentScale);


                    t.transform.localRotation = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                        ? Quaternion.LookRotation(point)
                        : Quaternion.LookRotation(point, Vector3.forward);
                });

               
                teeth.Add(tooth);
                //TODO: follow olmayan koşlda takip etmesin
            }
            
            SetTeethInfo(teeth.Count, Vector3.Distance(teeth[0].transform.position, teeth[1].transform.position));
        }
        
        
        private void SetTeethInfo(int teethCount, float toothUnit)
        {
            Data.TeethCount = teethCount;
            Data.ToothUnit = toothUnit;
        }

        public List<Tooth> teeth = new();

        void ResetTeeth()
        {
            if (teeth.Count == 0)
            {
                print("zero");
                if (Application.isEditor)
                {
                    List<Tooth> deadTeeth = _teethParent.GetComponentsInChildren<Tooth>().ToList();
                    deadTeeth.ForEach(t=>
                        {
                            t.transform.SetParent(ToothPool.Instance.transform);
                            ToothPool.Instance.ReleaseItem(t);
                        });
                }
            }

           
           
            teeth.ForEach(t=>
            {
                t.transform.SetParent(ToothPool.Instance.transform);
                ToothPool.Instance.ReleaseItem(t);
            });
            teeth.Clear();
        }

        private void OnDisable()
        {
            ChainEvents.OnCogDataSet -= ReadyForTeethCreation;
            ChainEvents.OnPoolCreated -= WaitForPool;

        }
    }
}