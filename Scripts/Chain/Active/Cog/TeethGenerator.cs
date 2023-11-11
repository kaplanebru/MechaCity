using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class TeethGenerator : MonoBehaviour
    {
        public List<Tooth> teeth = new();
        [SerializeField] private TeethPool teethPool;

        private CogData Data;
        public Tooth toothPb;
        float _intervalAngle = 60;
        [SerializeField] private bool hasTeeth;

        private void OnEnable()
        {
            if (Application.isPlaying) return;

            teethPool = GetComponentInChildren<TeethPool>();
            ChainEvents.OnDeleteTeeth += ClearTeeth;
            ChainEvents.OnCogDataSet += ReadyForTeethCreation;
           
            //ChainEvents.OnPoolReady += ReadyForTeethCreation;
        }

        private void ClearTeeth()
        {
            teeth.Clear();
        }


        void ReadyForTeethCreation(CogData data, Transform cogTransform)
        {
            if (cogTransform.position != transform.position) return;

            Data = data;
            CreateTeeth();
        }


        void SetIntervalAngle()
        {
            _intervalAngle = Data.ToothGap / (Data.Radius);
            if (Data.Equalize)
                _intervalAngle = TrigonometryHelper.Angle360(_intervalAngle);

            if (_intervalAngle < Data.MinGapLimit)
                _intervalAngle = Data.MinGapLimit;
        }


        public void CreateTeeth() //CogData data, Transform teethParent //params object[] args
        {
            ResetTeeth3(); //!her event için resetlenemez, butona basıldığında resetlenebilir.
            Vector3 inverseParentScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y,
                1f / transform.localScale.z);
            SetIntervalAngle();

            for (float i = 0; i < 360; i += _intervalAngle)
            {
                Vector3 point = TrigonometryHelper.CirclePoint(i, Data.Radius);

                Tooth tooth = teethPool.GetItem(t =>
                {
                    t.transform.position = transform.position + transform.rotation * point;
                   // t.transform.parent = _teethParent;
                    t.transform.localScale = Vector3.Scale(Data.toothScale, inverseParentScale);
                    t.transform.localRotation = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                        ? Quaternion.LookRotation(point)
                        : Quaternion.LookRotation(point, Vector3.forward);
                });
                
                teeth.Add(tooth);

                //tooth.transform.SetParent(_teethParent); //BUG FİX : teethparenttan patlıyormuş. prefab ve scene moddan da patlıyor olabilir.transform yerine eventte this denebilir
            }

            SetTeethInfo(teeth.Count, Vector3.Distance(teeth[0].transform.position, teeth[1].transform.position));
        }

        


        private void SetTeethInfo(int teethCount, float toothUnit)
        {
            Data.TeethCount = teethCount;
            Data.ToothUnit = toothUnit;
        }

        public void ResetTeeth3()
        {
            teethPool = GetComponentInChildren<TeethPool>();


            if (teethPool == null) //for bug check, temporary
            {
                Debug.LogError("teeth pool null");
                return;
            }

            if (teethPool.pool.Count == 0)
            {
                teethPool.ActivatePool(0, toothPb);
            }

            if (teeth.Count > 0 && teeth[0] == null)
                teeth.Clear();

            teeth.ForEach(t =>
            {
                //t.transform.SetParent(teethPool.transform);
                teethPool.ReleaseItem(t);
            });
            teeth.Clear();
        }

       

        public void DeleteTeeth()
        {
            ResetTeeth3();

            for (int i = teeth.Count - 1; i >= 0; i--)
            {
                var tooth = teeth[i];
                teeth.Remove(tooth);
                DestroyImmediate(tooth.gameObject, true);
            }
        }

        

        private void OnDisable()
        {
            if (Application.isPlaying) return;

            ChainEvents.OnDeleteTeeth -= ClearTeeth;
            ChainEvents.OnCogDataSet -= ReadyForTeethCreation;
        }
    }
}