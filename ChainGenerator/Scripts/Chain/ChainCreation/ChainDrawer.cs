using System.Collections.Generic;
using UnityEngine;


namespace Chain
{
   
    public class ChainDrawer : IChainGenerator
    {
       
        public ChainGeneratorData Data { get; set; }
        
        public ChainDrawer(ChainGeneratorData data)
        {
            Data = data;
        }

        private List<ChainLink> _links = new();
        private int _pointsCount;
        public Transform lastLinkPrefab;
        private IChainGenerator _chainGeneratorImplementation;


        List<ChainLink> DrawChain()
        {
            _pointsCount = Data.ChainPoints.Count;
            CreateLinks();
            return _links;
        }
        
        
        public List<ChainLink> ExecutePhase()
        {
            return DrawChain();
        }


        void CreateLinks()
        {
            if(Data.Pool.pool.Count < _pointsCount)
            {
                Debug.LogWarning("Pool link amount is not enough");
                _links.ForEach(l=>Data.Pool.ReleaseItem(l));
                _links.Clear();
                Data.Pool.ActivatePool();
                return;
            }
            for (int i = 0; i < _pointsCount; i++)
            {
                var link = Data.Pool.GetItem();
                link.transform.localPosition = Data.ChainPoints[i];
                link.transform.localScale = Data.ChainData.LinkSize;
                
                _links.Add(link);
                SetLookRotations(i, link);
            }
            
        }

        void SetLookRotations(int i, ChainLink newLink)
        {
            if (i < _pointsCount)
            {
                newLink.transform.localRotation =
                    Quaternion.LookRotation((Data.ChainPoints[(i + 1) % _pointsCount] - Data.ChainPoints[i]).normalized);
            }

            if (Data.ChainData.LinkRotationEffect)
                RotateLinks(i, newLink);
        }

        void RotateLinks(int i, ChainLink link)
        {
            var rot = link.transform.localRotation;
            if (i % 2 == 0)
                link.transform.localRotation =
                    Quaternion.Euler(rot.eulerAngles.x,
                        rot.eulerAngles.y,
                        rot.eulerAngles.z - 90);
        }
    }
}


#region LastLink

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
// public dynamic ExecutePhase<T>() where T : new()
// {
//     
// }


#endregion
   
      