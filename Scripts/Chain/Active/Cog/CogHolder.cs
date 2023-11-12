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
        public List<Cogwheel> cogs;
        public int newCogIndex = 0;


        private void OnEnable()
        {
            cogs = GetRestoredCogs().ToList();
        }

        public Cogwheel[] GetRestoredCogs()
        {
            return GetComponentsInChildren<Cogwheel>();
        }

        public Cogwheel[] AddCog(Cogwheel newCog)
        {
            cogs.Add(newCog);
            return cogs.ToArray();
        }

        public Cogwheel[] RemoveCog(Cogwheel cogToRemove)
        {
            cogs.Remove(cogToRemove);
            
            return cogs.ToArray();
        }

        public Cogwheel[] GetChainRelatedCogs()
        {
            return cogs.Where(c=>c.Data.ContactType == ChainEnums.CogContactType.ChainRelated).ToArray();
        }

        private void Start()
        {
           // ChainEvents.OnCogsUpdated?.Invoke(cogs.ToArray());

        }
    }
}

