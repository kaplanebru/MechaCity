using System.Collections.Generic;
using System.Linq;
using MyNamespace;
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


        private List<Vector3> _chainPoints = new();
        [SerializeField]private List<ChainLink> _links = new();
        private int _pointsCount;
        private LinksPool linksPool;

        private void OnEnable()
        {
            linksPool = GetComponentInChildren<LinksPool>();
            Data = GetComponent<ChainSpawner>().Data; //TODO: TEST
            ChainEvents.OnPointsCreated += DrawChain;
            ResetValues();
        }

        void DrawChain(List<Vector3> points)
        {
            print("draw chain");
            _chainPoints = points;
            _pointsCount = _chainPoints.Count;
            if (Data.Type == ChainEnums.ChainType.Line)
                DrawLines();
            else
                CreateLinks();
        }

        void CreateLinks()
        {
            print("create links");
            ResetLinks();
            print(_pointsCount);
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
            
          

            // if(Data.Type == ChainType.BikeChain)
            //     RegulateLastLink();

            ChainEvents.OnLinksCreated?.Invoke(_links);
        }


        void ResetLinks()
        {
            // _links.Clear();
            // _links = linksPool.GetComponentsInChildren<ChainLink>(false).ToList();
            _links.ForEach(l=> linksPool.ReleaseItem(l));
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

        void ResetValues()
        {
            _chainPoints.Clear();
            lr.positionCount = 0;
        }

        private void OnDisable()
        {
            ChainEvents.OnPointsCreated -= DrawChain;
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