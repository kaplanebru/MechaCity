using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pool<T> : MonoBehaviour where T : Component
{
    public static Pool<T> Instance;
    private Queue<T> _pool = new Queue<T>();

    public T GetItem()
    {
        T itemFromPool = _pool.Dequeue(); //sıranın BAŞINDAN alma
        
        itemFromPool.gameObject.SetActive(true);
        
        //_pool.Enqueue(itemFromPool); //sıranın SONUNA ekleme
        return itemFromPool; 
    }
    
    public void ReleaseItem(T item)
    {
        if (_pool.Contains(item))
        {
            item.gameObject.SetActive(false); 
            _pool.Enqueue(item);
        }
    }
    
    public void CreatePool(int amount, Transform poolParent, T prefab)
    {
        for (int i = 0; i < amount; i++)
        {
            T item = Instantiate(prefab, poolParent);
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }
    }

    public void DisableAll()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            T item= GetItem();
            item.gameObject.SetActive(false);
        }
    }
    
    public void GetAll(float interval, int population, int startOffset)
    {
        for (int i = population - 1; i >= 0; i--)
        {
            T item= GetItem();
            // var dist = Vector3.forward * (i * interval -interval*startOffset);  //startOffset pooldaki elementler yetmediği için
            // item.transform.localPosition += dist;
        }
    }
}
