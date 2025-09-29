using UnityEngine;

namespace Chain
{
    public abstract class BaseTransposable : MonoBehaviour
    {
        public abstract void Initialize();

        public virtual void Stop()
        {
            //StopAllCoroutines();
        }
    }

}
