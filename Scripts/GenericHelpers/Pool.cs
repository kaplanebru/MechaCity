using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GenericHelper
{
    //[ExecuteAlways]
    public abstract class Pool<T> : MonoBehaviour where T : Component
    {
        
        public static Pool<T> Instance;
        [SerializeField]public Queue<T> pool = new Queue<T>();

        
        public T GetItem(Action<T> callback = null)
        {
            T itemFromPool = pool.Dequeue(); //sıranın BAŞINDAN alma, sıradan çıkartma

            callback?.Invoke(itemFromPool);
        
            itemFromPool.gameObject.SetActive(true);
            return itemFromPool;
        }
    
        public void ReleaseItem(T item)
        {
            item.gameObject.SetActive(false);
            pool.Enqueue(item); //sıraya ekleme (SONDAN))
        }
        
        public void CreatePool(int amount, Transform poolParent, T prefab)
        {
            for (int i = 0; i < amount; i++)
            {
                T item = Instantiate(prefab, poolParent);
                item.gameObject.SetActive(false);
                item.transform.SetParent(transform);
                pool.Enqueue(item);
            }
            print(pool.Count);
        }
        


        public void RestorePool(T[] items)
        {
            pool.Clear();
            foreach (T item in items)
            {
                if(!item.gameObject.activeInHierarchy) 
                    pool.Enqueue(item);
                //ReleaseItem(item);
            }
        }
    }
}