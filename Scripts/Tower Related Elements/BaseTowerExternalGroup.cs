using System.Collections;
using System.Collections.Generic;
using TowerExternal;
using UnityEngine;

namespace TowerExternal
{
    public abstract class BaseTowerRelatedCollection<T> : ITowerRelatedCollection where T : ITowerRelatedElement
    {
        protected Dictionary<int, T> Collection { get; } = new();
    
        public BaseTowerRelatedCollection(T[] collection)
        {
            foreach (var item in collection)
            {
                if (Collection.ContainsKey(item.Id))
                {
                    Debug.Log(item.Id);
                }
                else
                {
                    Collection.Add(item.Id, item);
                }
            }
        }

        public virtual void Subscribe() {}

        public virtual void Unsubscribe() {}
    }
}
