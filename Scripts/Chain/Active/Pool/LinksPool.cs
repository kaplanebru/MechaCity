using System.Linq;
using GenericHelper;
using UnityEngine;

namespace Chain
{
    [ExecuteInEditMode]
    public class LinksPool : Pool<ChainLink>
    {
        public void ActivatePool()
        {
            //print("activate pool");
            if (pool.Count > 0) return;
            //print("activated");
            
            var poolChildren = GetComponentsInChildren<ChainLink>(true).ToList();
            var childrenLength = poolChildren.Count;

            if (childrenLength > 0)
                RestorePool(poolChildren.ToArray());
            
        }
        
        public void DeletePool()
        {
            DestroyImmediate(gameObject, true);
        }

        // public void DeleteLinks()
        // {
        //     ChainEvents.OnDeleteLinks?.Invoke();
        //     DestroyImmediate(gameObject, true);
        //     
        // }
    }
}