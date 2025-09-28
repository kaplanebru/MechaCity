using System.Linq;
using ChainInGame;
#if UNITY_EDITOR
using ChainEditorHelper;
#endif
using UnityEditor;
using UnityEngine;


namespace Chain
{
    [ExecuteAlways]
    public class Machinery : MonoBehaviour
    {
        [HideInInspector] public bool isChainRelated = false;
        [HideInInspector] public float machinerySpeed = 10;

        [HideInInspector] public ChainGenerator chainGenerator;
        [HideInInspector] public CogHolder cogHolder;

        private Mover[] _movers;
        private IMachinePart[] _machineParts;
        public HoleAssetHolder holeAssetHolder;
        [HideInInspector] public int sortingOrder = 0;
        
        private bool _isMoving = false;
        public bool movingAtStart = true;

        

        private void OnEnable()
        {
            if (!Application.isPlaying)
                GetObjects();
            if (Application.isPlaying)
                SetMovers();
            
            ChainEvents.OnMovieClipBegin += Move;
        }

        private void Start()
        {
            #if UNITY_EDITOR
            MyPrefabHelpers.UnpackPrefabInstance(gameObject);
            #endif
            
            if(movingAtStart)
                Move();
        }

        
        public void Move()
        {
            if (Application.isPlaying)
            {
                if(_isMoving) return;
                foreach (var mover in _movers)
                {
                    if (mover is ChainMover)
                        if (!isChainRelated)
                            continue;

                    mover.StartMotion();
                }

                _isMoving = true;
            }
        }
        
        public void StopMovers()
        {
            if(!_isMoving) return;
            foreach (var mover in _movers)
            {
                if (mover is ChainMover)
                    if (!isChainRelated)
                        continue;
                mover.StopMotion();
            }

            _isMoving = false;
        }

        public void To2D()
        {
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        void GetObjects()
        {
            _machineParts = GetComponentsInChildren<IMachinePart>();
            cogHolder = GetComponentInChildren<CogHolder>();
            chainGenerator = (ChainGenerator) _machineParts.FirstOrDefault(m => m is ChainGenerator);
            cogHolder.GetCogs(_machineParts.OfType<Cogwheel>());
            cogHolder.cogs.ForEach(c=>c.sortingOrder = sortingOrder);
        }

        public void SetMovers()
        {
            _machineParts = GetComponentsInChildren<IMachinePart>();
            _movers = GetComponentsInChildren<Mover>();

            var motionDirection =
                isChainRelated ? chainGenerator.ChainData.motionDirection : ChainEnums.ChainDirection.None;

            for (var i = 0; i < _movers.Length; i++)
            {
                var mover = _movers[i];
                if (mover is ChainMover chainMover)
                {
                    if (!isChainRelated) continue;
                    chainMover.Setup(chainGenerator.links, chainGenerator.cogAmount);
                }

                mover.MachinerySetup(machinerySpeed, gameObject.GetInstanceID(), _machineParts[i].GetMoverData(),
                    motionDirection);
            }
        }

        public void SetPivotToSelectedGear(int i)
        {
            int cogCount = cogHolder.cogs.Count;
            Vector3 pivotOffset = cogHolder.cogs[i].transform.localPosition;

            for (int j = 0; j < cogCount; j++)
            {
                Vector3 newPosition = (cogHolder.cogs[j].transform.localPosition - pivotOffset);
                cogHolder.cogs[j].transform.localPosition = newPosition;
            }

            transform.position = cogHolder.cogs[i].transform.position;
            transform.rotation = cogHolder.cogs[i].transform.rotation;
        }

#if UNITY_EDITOR
        public void SaveOnExistingPrefab()
        {
            if (MyPrefabHelpers.IsPrefabInstance(gameObject))
                MyPrefabHelpers.OverrideChanges(gameObject);
            else
            {
                var path = PathHelper.FindPathByName(name);
                if (path == null)
                    return;

                GameObject newInstance = Instantiate(gameObject);
                PrefabUtility.SaveAsPrefabAsset(newInstance, path);

                DestroyImmediate(newInstance);
                SaveMachinery();
            }
        }

        public void SaveMachinery()
        {
            Debug.Log("Chain machinery saved");

            if (isChainRelated && chainGenerator.ChainData != null)
                EditorUtility.SetDirty(chainGenerator.ChainData);
            EditorUtility.SetDirty(gameObject); //

            if (MyPrefabHelpers.IsPrefabInstance(gameObject))
                MyPrefabHelpers.OverrideChanges(gameObject);
        }

        public void DeleteLinkPool()
        {
            if (MyPrefabHelpers.IsPrefabInstance(gameObject))
            {
                Debug.LogWarning("Change pool from prefab view");
                return;
            }

            chainGenerator.DeletePoolClearLinks();
            SaveMachinery();
            chainGenerator.CreatePool();
        }

        public void DeleteTeethPool(int i)
        {
            if (MyPrefabHelpers.IsPrefabInstance(gameObject))
            {
                Debug.LogWarning("Change pool from prefab view");
                return;
            }

            cogHolder.cogs[i].DeletePool();
            SaveMachinery();
            cogHolder.cogs[i].CreateNewPool();
        }
        
#endif

        private void OnDisable()
        {
            ChainEvents.OnMovieClipBegin -= Move;
        }
    }
}