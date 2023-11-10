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
        public int newCogIndex = 0;


        private void OnEnable()
        {
            cogs = GetComponentsInChildren<Cogwheel>().ToList(); 
        }

        public List<Cogwheel> GetChainRelatedCogs()
        {
            //return GetComponentsInChildren<Cogwheel>().Where(c=>c.Data.ContactType == ChainEnums.CogContactType.ChainRelated).ToList();
            print(cogs.Count);
            return GetComponentsInChildren<Cogwheel>().Where(c=>c.Data.ContactType == ChainEnums.CogContactType.ChainRelated).ToList();
        }

        private void Start()
        {
           // ChainEvents.OnCogsUpdated?.Invoke(cogs.ToArray());

        }
    }
}

