using System;
using System.Collections.Generic;
using System.Linq;
using MyNamespace;
using UnityEditor;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class ChainDrawer : MonoBehaviour
    {
        public ChainData Data;
        public LineRenderer lr;
        Material lrMat;
        public Material firstCubeMaterial;
        public Transform lastLinkPrefab;
        [SerializeField] ChainLink linkPrefab; //temp 


        [SerializeField] private List<Vector3> _chainPoints = new();
        [SerializeField] private List<ChainLink> _links = new();
        private int _pointsCount;
        [SerializeField] private LinksPool linksPool;

        private void OnEnable()
        {
            if (!Application.isPlaying)
            {
                linksPool = GetComponentInChildren<LinksPool>();
                Data = GetComponent<ChainSpawner>().Data;

                ChainEvents.OnPointsCreated += DrawChain;
                ChainEvents.OnDeleteLinks += ClearLinks;
                ChainEvents.OnLinksPoolUpdated += GetLinksPool;
            }
        }

        public void GetLinksPool(LinksPool pool)
        {
            linksPool = pool;
        }


        private void Start()
        {
            ChainEvents.OnLinksCreated?.Invoke(_links, _chainPoints); //after edit, for mover
        }

        void ClearLinks()
        {
            _links.Clear();
        }

        public void DrawChain(List<Vector3> points)
        {
           // print("draw chain");
            //_chainPoints.Clear(); //todo: bazen buga sebep oluyor
            _chainPoints = points;
            _pointsCount = _chainPoints.Count;
            if (Data.Type == ChainEnums.ChainType.Line)
                DrawLines();
            else
                CreateLinks();
        }

        void CreateLinks()
        {
            ResetLinks();
           // return;
            
            for (int i = 0; i < _pointsCount; i++)
            {
                //var link = LinkPool.Instance.GetItem(l => l.transform.position = _chainPoints[i]);
                //var link = Instantiate(linkPrefab, transform.GetChild(1));
                //link.transform.localPosition = _chainPoints[i];
                var link = linksPool.GetItem(l => l.transform.localPosition = _chainPoints[i]);

                SetLookRotations(i, link.transform);
                // if (Data.Type == ChainEnums.ChainType.StandardChain)
                //     RotateLinks(i, link);

                _links.Add(link);

                if (i == 0)
                    link.GetComponentInChildren<MeshRenderer>().material = firstCubeMaterial; //debug
            }


            EditorUtility.SetDirty(GetComponentInParent<Machinery>().gameObject);

            // if(Data.Type == ChainType.BikeChain)
            //     RegulateLastLink();
            //

            //ChainEvents.OnLinksCreated?.Invoke(_links);
        }


        void ResetLinks()
        {
            // _links.Clear();
            // _links = linksPool.GetComponentsInChildren<ChainLink>(false).ToList();

           // if(linksPool == null)
                linksPool = GetComponentInChildren<LinksPool>();
            
          //  print(linksPool.name);

          if (linksPool == null)  //for bug check, temporary
          {
              Debug.LogError("links pool null");
              return;
          }
            if (linksPool.pool.Count == 0)
            {
                print("chain points count: "+_chainPoints.Count);
                linksPool.ActivatePool(_chainPoints.Count, Data.linkPrefab);
            }

            if (_links.Count > 0 && _links[0] == null)
                _links.Clear();

            _links.ForEach(l => linksPool.ReleaseItem(l));
            _links.Clear();
        }


        void SetLookRotations(int i, Transform newObj)
        {
            if (i < _pointsCount)
            {
                newObj.transform.rotation = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                    ? Quaternion.LookRotation(_chainPoints[(i + 1) % _pointsCount] - _chainPoints[i])
                    : Quaternion.LookRotation(_chainPoints[(i + 1) % _pointsCount] - _chainPoints[i], Vector3.forward);
            }
        }


        void DrawLines()
        {
            lr.positionCount = _chainPoints.Count;
            lr.SetPositions(_chainPoints.ToArray());
        }

        void ResetValues() //editörde hata veriyor
        {
            _chainPoints.Clear();
            lr.positionCount = 0;
        }

        private void OnDisable()
        {
            if (!Application.isPlaying)
            {
                ChainEvents.OnPointsCreated -= DrawChain;
                ChainEvents.OnDeleteLinks -= ClearLinks;
                ChainEvents.OnLinksPoolUpdated -= GetLinksPool;

            }
        }

        // void RotateLinks(int i, Transform newObj)
        // {
        //     var rot = newObj.transform.rotation;
        //     if (i % 2 == 0)
        //         newObj.transform.rotation = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
        //             ? Quaternion.Euler(rot.eulerAngles.x,
        //                 rot.eulerAngles.y,
        //                 rot.eulerAngles.z - 90)
        //             : Quaternion.Euler(rot.eulerAngles.x,
        //                 rot.eulerAngles.y - 90,
        //                 rot.eulerAngles.z);
        //
        // }
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
    }
}