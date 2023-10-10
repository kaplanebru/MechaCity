using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    public class LinkSpawner : MonoBehaviour
    {
        [SerializeField] int population = 100;
        [SerializeField] private Transform linkPrefab;
        
        
        private void Start()
        {
            Initialize();
        }
    
        private protected virtual void Initialize()
        {
            LinkPool.Instance.CreatePool(population, transform, linkPrefab);
        }
    }

}
