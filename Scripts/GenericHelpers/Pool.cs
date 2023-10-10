using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GenericHelper
{
    public abstract class Pool<T> : MonoBehaviour where T : Component
    {
        public static Pool<T> Instance;
        private Queue<T> pool = new Queue<T>();

        public T GetItem(Action<T> callback = null)
        {
            T itemFromPool = pool.Dequeue(); //sıranın BAŞINDAN alma, sıradan çıkartma

            callback?.Invoke(itemFromPool);
        
            itemFromPool.gameObject.SetActive(true);
            return itemFromPool;
        }
    
        public void ReleaseAndDeactivateItem(T item)
        {
            item.gameObject.SetActive(false);
            pool.Enqueue(item); //sıraya ekleme (SONDAN)
        }

        public void ReleaseItem(T item)
        {
            pool.Enqueue(item);
        }

        public void CreatePool(int amount, Transform poolParent, T prefab)
        {
            for (int i = 0; i < amount; i++)
            {
                T item = Instantiate(prefab, poolParent);
                item.gameObject.SetActive(false);
                pool.Enqueue(item);
            }
        }

        public void ReleaseAll()
        {
            for (int i = 0; i < pool.Count; i++)
            {
                T item = GetItem(); //possible bug: hatalı olabilir, tekrar get ediyor, başka objeyi get ediypr yani. ve pool count kadarını almamış da olabilir
                ReleaseAndDeactivateItem(item);
            }
        }
    }
}
