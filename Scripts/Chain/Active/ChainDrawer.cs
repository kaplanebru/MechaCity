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
        private int _pointsCount;
        [SerializeField] private LinksPool linksPool;

        private void OnEnable()
        {
            
            if (!Application.isPlaying)
            {
                linksPool = GetComponentInParent<Machinery>().GetComponentInChildren<LinksPool>(); //TODO: linkspool her silindiğinde machineryde update oluyor mu
                Data = GetComponent<ChainSpawner>().Data;

                //ChainEvents.OnPointsCreated += DrawChain;
                //ChainEvents.OnDeleteLinks += ClearLinks;
            }
        }
        
        

        public void GetLinksPool(LinksPool pool)
        {
            linksPool = pool;
        }


        private void Start()
        {
            //ChainEvents.OnLinksCreated?.Invoke(_links, _chainPoints); //after edit, for mover
        }

        void ClearLinks()
        {
            _links.Clear();
        }

        public void DrawChain(List<Vector3> points)
        {
            _chainPoints = points;
            _pointsCount = _chainPoints.Count;
          
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
                var link = linksPool.GetItem(l =>
                {
                    l.transform.rotation = Quaternion.identity;
                    l.transform.position = _chainPoints[i];
                });

                SetLookRotations(i, link);

                _links.Add(link);
            }


            //EditorUtility.SetDirty(GetComponentInParent<Machinery>().gameObject);
            

            // if(Data.Type == ChainType.BikeChain)
            //     RegulateLastLink();

            
            //GetComponentInParent<Machinery>().ApplyChangesToPrefab();
           // ChainEvents.OnLinksReady?.Invoke();
            //ChainEvents.OnLinksCreated?.Invoke(_links);
        }


        public void ResetLinks()
        {
            if (linksPool == null)
            {
                linksPool = GetComponentInParent<Machinery>().GetComponentInChildren<LinksPool>();
                if (linksPool == null) //for bug check, temporary
                {
                    Debug.LogError("links pool null");
                    return;
                }
            }

            if (linksPool.pool.Count == 0)
            {
                linksPool.ActivatePool(_chainPoints.Count, Data.linkPrefab);
            }

            if (_links.Count > 0 && _links[0] == null)
                _links.Clear();

            _links.ForEach(l => linksPool.ReleaseItem(l));
            _links.Clear();
        }


        void SetLookRotations(int i, ChainLink newLink)
        {
            if (i < _pointsCount)
            {
                newLink.transform.rotation = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                    ? Quaternion.LookRotation((_chainPoints[(i + 1) % _pointsCount] - _chainPoints[i]).normalized)
                    : Quaternion.LookRotation((_chainPoints[(i + 1) % _pointsCount] - _chainPoints[i]).normalized,
                        Vector3.forward);
                //TODO: normalized sonradan eklendi, local silindi
            }

            if (Data.LinkRotationEffect)
                RotateLinks(i, newLink);
        }

        void RotateLinks(int i, ChainLink link)
        {
            var rot = link.transform.rotation;
            if (i % 2 == 0)
                link.transform.rotation = ChainSpawner.Upwards == ChainEnums.UpAxis.Z
                    ? Quaternion.Euler(rot.eulerAngles.x,
                        rot.eulerAngles.y,
                        rot.eulerAngles.z - 90)
                    : Quaternion.Euler(rot.eulerAngles.x,
                        rot.eulerAngles.y - 90,
                        rot.eulerAngles.z);
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
                ChainEvents.OnDeleteLinks -= ClearLinks;
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