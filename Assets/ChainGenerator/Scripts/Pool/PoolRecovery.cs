using System.Collections.Generic;
using GenericHelper;
using UnityEngine;

namespace Chain
{
    public class PoolRecovery<T, Q> : MonoBehaviour 
        where T : Pool<Q> 
        where Q : Component
    {
        private T poolObject;
        public T poolPrefab;

    
        public void DeletePoolObject()
        {
            DestroyImmediate(poolObject.gameObject, true);
        }
    
        void StartPool()
        {
            if (poolObject.pool != null) return;
            poolObject = GetComponentInChildren<T>();
            if (poolObject == null) poolObject = CreatePool();
        }

        public T CreatePool()
        {
            poolObject = Instantiate(poolPrefab, transform); //ChainData.LinksPoolPrefab
            return poolObject;
        }

        public void DeletePoolClearActiveItems(List<Q> usedItems) //aynı liste temizlenmeyecekse Action metodu da eklenebilir
        {
            usedItems.Clear(); // links.Clear();
            DeletePoolObject();
        }
    
        // public void ActivatePool()
        // {
        //     if (poolObject.pool.Count > 0) return;
        //         
        //     var allItems = GetComponentsInChildren<Q>(true).ToList();
        //     var itemsLength = allItems.Count;
        //
        //     if (itemsLength > 0)
        //         poolObject.RestorePool(allItems.ToArray());
        //         
        // }
        //  

    }

}
