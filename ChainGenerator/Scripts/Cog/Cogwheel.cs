using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class Cogwheel : MonoBehaviour, CogComponent, IMachinePart
    {
        public int id;
        public Transform parent;
        public GearData Data;
        public bool drawGizmos = false;
        public Transform cogObject;
        public Hole hole;
        public HoleHolder holeHolder;
        public int sortingOrder = 0;


        [SerializeField] List<Tooth> teeth = new();
        [SerializeField] private TeethPool pool;
        public int CogId { get; set; }

        private void OnEnable()
        {
            parent = transform.parent;
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                StartPool();
                holeHolder = GetComponentInChildren<HoleHolder>();
            }
#endif
        }

        public IMachinePartData GetMoverData()
        {
            return (IMachinePartData) Data;
        }

        public void Setup()
        {
            SetCogRadius();
            SetHoleSizeAndType(); //hole size radiusla bağlantılı

            if (Data.WithoutTeeth) return;
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


        Hole GetHolesById(int id)
        {
            if (holeHolder != null)
            {
                return holeHolder.ShowHole(id);
            }
            else
            {
                return null;
            }
        }

        void SetHoleSizeAndType()
        {
            SetCogVolume();
            hole = GetHolesById(Data.HoleId);
            if (hole == null) return;

            var holeSize = (Data.Radius - Data.HoleSize); // * 2;

            Vector3 scale = hole.transform.localScale;

            scale.z = holeSize;
            scale.x = holeSize;
            scale.y = Data.HoleDepth;

            hole.transform.localScale = scale;
        }

        public void AddData(GearData data)
        {
            Data = data;
        }

        private TeethGenerator _teethGenerator;

        void GenerateTeeth()
        {
            _teethGenerator = new TeethGenerator(Data, pool, transform);
            _teethGenerator.ReleasePreviousTeeth(teeth);
            teeth = _teethGenerator.CreateTeeth();
            if (teeth == null)
            {
                Debug.LogWarning("Pool tooth amount is not enough");
                pool.ActivatePool();
                return;
            }
            Data.TeethCount = teeth.Count;
            if (Data.TeethCount < 2)
            {
                Debug.LogWarning("Not enough tooth for teeth generation/ or try decreasing the tooth interval");
                return;
            }

            Data.ToothUnit = Vector3.Distance(teeth[0].transform.position, teeth[1].transform.position);
        }

        void StartPool()
        {
            if (Data == null) return;
            if (Data.WithoutTeeth) return;
            if (pool != null)
                return;
            pool = GetComponentInChildren<TeethPool>();
            
            if (pool == null)
            {
                if (Data.TeethPoolPrefab == null)
                {
                    Debug.LogWarning("Fill pool section!");
                    return;
                }
                pool = CreatePool();
            }
        }

        TeethPool CreatePool()
        {
            return Instantiate(Data.TeethPoolPrefab, transform);
        }

        public void DeletePool()
        {
            if(teeth != null)
                teeth.Clear();
            if(pool != null)
                pool.DeletePool();
        }

        public void CreateNewPool()
        {
            if (pool != null) return;
            pool = CreatePool();
            GenerateTeeth();
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
    }
}