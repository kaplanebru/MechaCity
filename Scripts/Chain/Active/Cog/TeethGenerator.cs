using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class TeethGenerator : MonoBehaviour, CogComponent
    {
        public int Id { get; set; }

        public List<Tooth> teeth = new();

        private CogData Data;
        float _intervalAngle = 60;
        [SerializeField] private TeethPool pool;

        private void OnEnable()
        {
            if (Application.isPlaying) return;

            ChainEvents.OnDeleteTeethPool += DeletePool;
            ChainEvents.OnCogDataSet += ReadyForTeethCreation;
            ChainEvents.OnCreateTeethPool += CreateNewPool;
            pool = GetComponentInChildren<TeethPool>();

            //ChainEvents.OnPoolReady += ReadyForTeethCreation;
        }
        void ReadyForTeethCreation(CogData data, int id, string systemId)
        {
            if(systemId != Machinery.InstanceID) return;
            if(id != Id) return;

            Data = data;
            StartPool();
            CreateTeeth();
        }
        
        void StartPool()
        {
            pool = pool == null ? CreatePool() : GetComponentInChildren<TeethPool>();
        }
        
        TeethPool CreatePool()
        {
            return Instantiate(Data.TeethPoolPrefab, transform);
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

                Tooth tooth = pool.GetItem(t =>
                {
                    t.transform.position = transform.position + transform.rotation * point;
                    t.transform.localScale = Vector3.Scale(Data.toothScale, inverseParentScale);
                    t.transform.localRotation = Quaternion.LookRotation(point);
                });
                
                teeth.Add(tooth);
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
            if (pool == null) //for bug check, temporary
            {
                Debug.LogError("teeth pool null");
                return;
            }

            if (pool.pool.Count == 0)
                pool.ActivatePool();

            if (teeth.Count > 0 && teeth.Any(t=>t==null))
                teeth.Clear();

            teeth.ForEach(t => pool.ReleaseItem(t));
            ClearTeeth();
        }
        
        private void DeletePool(int id, string systemId)
        {
            if(systemId != Machinery.InstanceID) return;
            if(id != Id) return;
            
            ClearTeeth();
            pool.DeletePool();
        }

        void CreateNewPool(int id, string systemId) 
        {
            if(systemId != Machinery.InstanceID) return;
            if(id != Id) return;
            
            pool = CreatePool();
        }
        
        private void ClearTeeth()
        {
            teeth.Clear();
        }

        private void OnDisable()
        {
            if (Application.isPlaying) return;

            ChainEvents.OnDeleteTeethPool -= DeletePool;
            ChainEvents.OnCogDataSet -= ReadyForTeethCreation;
            ChainEvents.OnCreateTeethPool -= CreateNewPool;
        }

    }
}