using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class ChainDrawer : MonoBehaviour
    {
        public ChainData Data;


        public Transform lastLinkPrefab;

        public List<Vector3> _chainPoints = new();
        public List<ChainLink> _links = new();
        [SerializeField]private int _oldPointsCount;
        private int _pointsCount;
        public LinksPool pool;

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                Data = GetComponent<ChainSpawner>().Data;
                StartPool();

                //ChainEvents.OnPointsCreated += DrawChain;
                //ChainEvents.OnDeleteLinks += ClearLinks;
            }
        }

        void StartPool()
        {
            if (pool != null)
                return;
            print("pool null");
            pool = GetComponentInChildren<LinksPool>();
            if (pool == null) pool = CreatePool();
        }

        public LinksPool CreatePool()
        {
            pool = Instantiate(Data.LinksPoolPrefab, transform);
            _oldPointsCount = 0;
            return pool;
        }


        private void Start()
        {
            //ChainEvents.OnLinksCreated?.Invoke(_links, _chainPoints); //after edit, for mover
        }

        public void DeletePoolClearLinks() //on delete links
        {
            _links.Clear();
            _oldPointsCount = 0;
            pool.DeletePool();
        }

        public void DrawChain(List<Vector3> points)
        {
            _chainPoints = points;
          
            _pointsCount = _chainPoints.Count;
            CheckPointAmountDifference();
            

            CreateLinks();
            _oldPointsCount = _pointsCount;
        }

        void CheckPointAmountDifference()
        {
            if(PoolNull()) return;
            if (_pointsCount < _oldPointsCount)
            {
                int rest = _oldPointsCount - _pointsCount;
                for (int i = 0; i < rest; i++)
                {
                    pool.ReleaseItem(_links.Last());
                    _links.Remove(_links.Last());
                }
            }
            else
            {
                int requiredAmount = _pointsCount - _oldPointsCount;
                for (int i = 0; i < requiredAmount; i++)
                {
                    _links.Add(pool.GetItem());
                }
            }
        }

        void CreateLinks()
        {
            //ResetLinks();
         

            for (int i = 0; i < _pointsCount; i++)
            {
                //var link = LinkPool.Instance.GetItem(l => l.transform.position = _chainPoints[i]);
                //var link = Instantiate(linkPrefab, transform.GetChild(1));
                //link.transform.localPosition = _chainPoints[i];
                
                // var link = pool.GetItem(l =>
                // {
                //     //l.transform.localRotation = Quaternion.identity;
                //     // l.transform.localRotation = Quaternion.Euler(Vector3.zero);
                //     l.transform.localPosition = _chainPoints[i];
                // });
                
                _links[i].transform.localPosition = _chainPoints[i];

                SetLookRotations(i, _links[i]);

                //_links.Add(link);
            }


            //EditorUtility.SetDirty(GetComponentInParent<Machinery>().gameObject);

            // if(Data.Type == ChainType.BikeChain)
            //     RegulateLastLink();

            //GetComponentInParent<Machinery>().ApplyChangesToPrefab();
            // ChainEvents.OnLinksReady?.Invoke();
            //ChainEvents.OnLinksCreated?.Invoke(_links);
        }


        bool PoolNull()
        {
            if (pool == null)
            {
                pool = GetComponentInParent<Machinery>().GetComponentInChildren<LinksPool>();
                if (pool == null) //for bug check, temporary
                {
                    Debug.LogError("links pool null");
                    return true;
                }
            }

            if (pool.pool.Count == 0)
                pool.ActivatePool(_chainPoints.Count, Data.linkPrefab);
            

            if (_links.Count > 0 && _links.Any(l => l == null))
            {
                _links.Clear();
                Debug.LogError("links null");
                //return true;
            }
                
            return false;
        }
        public void ResetLinks()
        {
            if(PoolNull()) return;

            _oldPointsCount = 0;
            _links.ForEach(l => pool.ReleaseItem(l));
            _links.Clear();
        }


        void SetLookRotations(int i, ChainLink newLink)
        {
            if (i < _pointsCount)
            {
                newLink.transform.localRotation =
                    Quaternion.LookRotation((_chainPoints[(i + 1) % _pointsCount] - _chainPoints[i]).normalized);
                //TODO: normalized sonradan eklendi, local silindi
            }

            if (Data.LinkRotationEffect)
                RotateLinks(i, newLink);
        }

        void RotateLinks(int i, ChainLink link)
        {
            var rot = link.transform.rotation;
            if (i % 2 == 0)
                link.transform.rotation =
                    Quaternion.Euler(rot.eulerAngles.x,
                        rot.eulerAngles.y,
                        rot.eulerAngles.z - 90);
        }


        void DrawLines()
        {
            // lr.positionCount = _chainPoints.Count;
            // lr.SetPositions(_chainPoints.ToArray());
        }

        void ResetValues() //editörde hata veriyor
        {
            // _chainPoints.Clear();
            // lr.positionCount = 0;
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
            {
                ChainEvents.OnPointsCreated -= DrawChain;
                ChainEvents.OnDeleteLinks -= DeletePoolClearLinks;
            }
        }
    }
}

//
// void RegulateLastLink()
// {
//     if (Vector3.Distance(_chainPoints.First(), _chainPoints.Last()) > Data.Unit)
//     {
//         var lastLink = _links.Last();
//         Vector3 dir = (_links.First().position - lastLink.transform.position).normalized;
//         var newLastLink = Instantiate(lastLinkPrefab, lastLink.transform.position + dir , lastLink.transform.rotation);
//         _links.Add(newLastLink);
//         _chainPoints.Add(newLastLink.transform.position);
//     }
// }
//
// public void ReleaseChain()
// {
//     _links.ForEach(l => LinkPool.Instance.ReleaseItem(l));
// }