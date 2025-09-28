using System.Linq;
using GenericHelper;
using UnityEngine;


namespace Chain
{
    [ExecuteInEditMode]
    public class TeethPool : Pool<Tooth>
    {
        public void ActivatePool()
        {
            if (pool.Count > 0) return;
            
            var poolChildren = GetComponentsInChildren<Tooth>(true).ToList();
            var childrenLength = poolChildren.Count;

            if (childrenLength > 0)
                RestorePool(poolChildren.ToArray());
            
        }
        
        public void DeletePool()
        {
            DestroyImmediate(gameObject, true);
        }
    }

}
