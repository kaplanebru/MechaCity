using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class Cogwheel : MonoBehaviour, CogComponent
    {
        public CogData Data;
        public bool drawGizmos = false;
        public Transform cogObject;
        public Hole[] holes;
        

        [SerializeField] List<Tooth> teeth = new();
        [SerializeField] private TeethPool pool;
        public int Id { get; set; }

        private void OnEnable()
        {
            if (!EditorApplication.isPlaying)
            {
                ChainEvents.OnDeleteTeethPool += DeletePool;
                ChainEvents.OnCreateTeethPool += CreateNewPool;
            
                StartPool();
            }
         
        }


        public void Setup()
        {
            var radius = Data.Radius;
            var scale = Vector3.one;
            scale.x = radius * 2;
            scale.z = radius * 2;
            cogObject.transform.localScale = scale;

           
            SetHoleSize();
            StartPool();
            GenerateTeeth();
            
            //ChainEvents.OnCogStart?.Invoke(Data, teeth);
        }

        Hole[] GetHolesByType(ChainEnums.HoleType holeType)
        {
            var allHoles = GetComponentsInChildren<Hole>(true);
            return allHoles.Where(h =>
            {
                h.gameObject.SetActive(h.holeType == holeType);
                return h.holeType == holeType;
            }).ToArray();
        }

        void SetHoleSize()
        {
            Hole[] holes = GetHolesByType(Data.HoleType);
            var holeSize = (Data.Radius - Data.circularThickness) * 2;
            foreach (var hole in holes)
            {
                // Vector3 inverseParentScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y,
                //     1f / transform.localScale.z);

                Vector3 scale = hole.transform.localScale;

                scale.x = holeSize;
                scale.z = holeSize;
                scale.x = holeSize;

                hole.transform.localScale = scale; //Vector3.Scale(scale, inverseParentScale);
            }
        }

        public void AddData(CogData data)
        {
            Data = data;
        }
        
        void GenerateTeeth()
        {
            ResetTeeth();
            teeth = new TeethGenerator(Data, pool).CreateTeeth(transform);
            Data.TeethCount = teeth.Count;
            Data.ToothUnit = Vector3.Distance(teeth[0].transform.position, teeth[1].transform.position);
        }
        
        void StartPool()
        {
            
            //pool = pool == null ? CreatePool() : GetComponentInChildren<TeethPool>();
            if (pool != null)
                return;
           
            pool = GetComponentInChildren<TeethPool>();
            if (pool == null) pool = CreatePool();
        }

        TeethPool CreatePool()
        {
            return Instantiate(Data.TeethPoolPrefab, transform);
        }

        private void DeletePool(int id)
        {
            if (id != Id) return;
            teeth.Clear();
            pool.DeletePool();
        }

        void CreateNewPool(int id)
        {
            if (id != Id) return;
            pool = CreatePool();
        }
        
        public void ResetTeeth()
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
            teeth.Clear();
        }
        
        public void SetSpinDirectionByChain(ChainEnums.ChainDirection chainDirection)
        {
            Data.RotationDirection = chainDirection == ChainEnums.ChainDirection.Clockwise ? 1 : -1;
        }

        private void DrawGizmos()
        {
            Gizmos.color = Color.yellow;
            //Gizmos.DrawWireSphere(transform.position, Data.Radius + 2);
            Gizmos.DrawWireCube(transform.position, (Data.Radius * 2 * Vector3.one) + 5 * Vector3.one);
            //Gizmos.DrawCube(transform.position + Vector3.forward * (Data.Radius + 5), Vector3.one * 1f);
        }

        private void OnDrawGizmos()
        {
            if (drawGizmos)
                DrawGizmos();
        }
        
        private void OnDisable()
        {
            if (!EditorApplication.isPlaying)
            {
                ChainEvents.OnDeleteTeethPool -= DeletePool;
                ChainEvents.OnCreateTeethPool -= CreateNewPool;
            }
            
        }
    }
}