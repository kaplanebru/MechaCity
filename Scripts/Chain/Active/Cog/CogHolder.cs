using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chain
{
    [ExecuteInEditMode]
    public class CogHolder : MonoBehaviour
    {
        public List<Cogwheel> cogs = new();

        private void OnEnable()
        {
            cogs = GetComponentsInChildren<Cogwheel>().ToList();
        }

        private void Start()
        {
           // ChainEvents.OnCogsUpdated?.Invoke(cogs.ToArray());

        }
    }
}

