using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Grid;


namespace ProjectileHandler
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [ReadOnly]public int population;
        public Projectile projectilePrefab;
    
        private void Start()
        {
            Initialize();
            //StartCoroutine(nameof(SpawnBandRoutine));
        }
    
        private protected virtual void Initialize()
        {
            population = GameGrid.SlotAmount; // * 3; //TODO: max mermi sayısıyla çarp
            ProjectilePool.Instance.CreatePool(population, transform, projectilePrefab);
            //BandPool.Instance.GetAll(BandPool.Instance.size, population, startOffset);
        }
    
        // public IEnumerator SpawnBandRoutine()
        // {
        //     while (true)
        //     {
        //         ProjectilePool.Instance.GetItem();
        //         yield return new WaitForFixedUpdate();
        //     }
        // }
    }
}

