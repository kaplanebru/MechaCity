using System.Collections.Generic;
using UnityEngine;

namespace Chain
{
    [ExecuteInEditMode]
    public class Residual : MonoBehaviour
    {
        public List<Transform> residualChildren = new();

        private void OnEnable()
        {
            ChainEvents.OnDeleteObject += AddToResidual;
        }

        void AddToResidual(Transform child)
        {
            print("add to residue");
//            child.SetParent(transform);
            residualChildren.Add(child);
        }

        public void CleanResiduals()
        {
            if(residualChildren.Count == 0) return;
            for (int i = residualChildren.Count - 1; i >= 0; i--)
            {
                var temp = residualChildren[i];
                residualChildren.Remove(temp);
                DestroyImmediate(temp.gameObject);
            }
        }

        private void OnDisable()
        {
            ChainEvents.OnDeleteObject -= AddToResidual;
        }
    }
}

