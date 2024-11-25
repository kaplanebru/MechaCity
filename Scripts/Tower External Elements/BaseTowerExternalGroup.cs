using System.Collections;
using System.Collections.Generic;
using TowerExternal;
using UnityEngine;

namespace TowerExternal
{
    public abstract class BaseTowerExternalGroup<T> where T : ITowerExternal, ITowerRelated
    {
        protected Dictionary<int, T> Group { get; } = new();
    
        public BaseTowerExternalGroup(T[] group)
        {
            foreach (var item in group)
            {
                // var id = ((ITowerRelated) item).Id;
                Group.Add(item.Id, item);
            }
        }
    }
}
