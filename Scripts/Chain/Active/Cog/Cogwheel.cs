using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class Cogwheel : MonoBehaviour, CogComponent, IMachinePart
    {
        public CogData Data;
        public bool drawGizmos = false;
        public Transform cogObject;
        public Hole hole;
        private Hole[] allHoles;
        public HoleHolder holeHolder;


        [SerializeField] List<Tooth> teeth = new();
        [SerializeField] private TeethPool pool;
        public int CogId { get; set; }

        private void OnEnable()
        {
            if (!EditorApplication.isPlaying)
            {
                ChainEvents.OnDeleteTeethPool += DeletePool;
                ChainEvents.OnCreateTeethPool += CreateNewPool;

                StartPool();
                allHoles = GetComponentsInChildren<Hole>(true);
                holeHolder = GetComponentInChildren<HoleHolder>();
            }
        }


        public void Setup()
        {
            SetCogRadius();
            SetHoleSizeAndType(); //hole size radiusla bağlantılı

            StartPool();
            GenerateTeeth();
        }

        public void AccidentalSetup()
        {
            SetHoleSizeAndType(); //burda teethle ve chainle işimiz yok
        }


        void SetCogRadius()
        {
            var radius = Data.Radius;
            if (radius == 0) return;
            var scale = Vector3.one;
            scale.x = radius * 2;
            scale.z = radius * 2;
            cogObject.transform.localScale = scale;
        }

        void SetCogVolume()
        {
            var scale = cogObject.transform.localScale;
            scale.y = Data.Volume;
            cogObject.transform.localScale = scale;
        }


        void HoleDepth()
        {
            float multiplier = 1;

            var pos = hole.transform.localScale;
            pos.y = Data.HoleDepth * multiplier;
            multiplier *= -1;
            hole.transform.localScale = pos;
        }


        Hole GetHolesById(int id)
        {
            if (holeHolder != null)
            {
                return holeHolder.CreateHole(id);
            }
            else
            {
                return null;
            }
            // return allHoles.Where(h =>
            // {
            //     h.gameObject.SetActive(h.Id == id);
            //     return h.Id == id;
            // }).ToArray();
        }

        void SetHoleSizeAndType()
        {
            SetCogVolume();
            hole = GetHolesById(Data.HoleId);
            if (hole == null) return;

            var holeSize = (Data.Radius - Data.HoleSize) * 2;


            Vector3 scale = hole.transform.localScale;

            scale.z = holeSize;
            scale.x = holeSize;
            scale.y = Data.HoleDepth;

            hole.transform.localScale = scale; //Vector3.Scale(scale, inverseParentScale);


            //HoleDepth();
        }

        public void AddData(CogData data)
        {
            Data = data;
        }

        private TeethGenerator _teethGenerator;

        void GenerateTeeth()
        {
            // if(_toothScale != Data.toothScale && _teethGenerator != null)
            //     _teethGenerator.SetTeethSize();

            _teethGenerator = new TeethGenerator(Data, pool, transform);
            _teethGenerator.ReleasePreviousTeeth(teeth);
            teeth = _teethGenerator.CreateTeeth();
            Data.TeethCount = teeth.Count;
            if (Data.TeethCount < 2)
            {
                Debug.LogWarning("Not enough magnitude for teeth generation");
                return;
            }

            Data.ToothUnit = Vector3.Distance(teeth[0].transform.position, teeth[1].transform.position);
        }

        void StartPool()
        {
            //pool = pool == null ? CreatePool() : GetComponentInChildren<TeethPool>();
            if (pool != null)
                return;
            print("pool null");
            pool = GetComponentInChildren<TeethPool>();
            if (pool == null) pool = CreatePool();
        }

        TeethPool CreatePool()
        {
            return Instantiate(Data.TeethPoolPrefab, transform);
        }

        private void DeletePool(int id)
        {
            if (id != CogId) return;
            teeth.Clear();
            pool.DeletePool();
        }

        void CreateNewPool(int id)
        {
            if (id != CogId) return;
            pool = CreatePool();
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

        private void OnDrawGizmosSelected()
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

        public IMachinePartData GetMoverData()
        {
            return (IMachinePartData) Data;
        }
    }
}