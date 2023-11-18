using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
           // CheckPointAmountDifference();
            CreateLinks();
            return _links;
        }
        
        
        public List<ChainLink> ExecutePhase()
        {
            return DrawChain();
        }


        void CreateLinks()
        {
            for (int i = 0; i < _pointsCount; i++)
            {
                var link = Data.Pool.GetItem();
                link.transform.localPosition = Data.ChainPoints[i];
                
                _links.Add(link);
                SetLookRotations(i, link);
            }
            
            // if(Data.Type == ChainType.BikeChain)
            //     RegulateLastLink();
            
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
            var rot = link.transform.rotation;
            if (i % 2 == 0)
                link.transform.rotation =
                    Quaternion.Euler(rot.eulerAngles.x,
                        rot.eulerAngles.y,
                        rot.eulerAngles.z - 90);
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

// public dynamic ExecutePhase<T>() where T : new()
// {
//     
// }
      

// void CheckPointAmountDifference()
// {
//     if (_pointsCount < Data.OldPointsCount)
//     {
//         int rest = Data.OldPointsCount - _pointsCount;
//         for (int i = 0; i < rest; i++)
//         {
//             Data.Pool.ReleaseItem(_links.Last());
//             _links.Remove(_links.Last());
//         }
//     }
//     else
//     {
//         int requiredAmount = _pointsCount - Data.OldPointsCount;
//         for (int i = 0; i < requiredAmount; i++)
//         {
//             _links.Add(Data.Pool.GetItem());
//         }
//     }
// }