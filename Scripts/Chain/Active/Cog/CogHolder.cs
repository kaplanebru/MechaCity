using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Chain
{
    public interface CogComponent
    {
        int Id { get; set; }

        public void SetId(int id)
        {
            Id = id;
        }
    }

    [ExecuteInEditMode]
    public class CogHolder : MonoBehaviour
    {
        public List<Cogwheel> cogs;
        public int newCogIndex = 0;


        private void OnEnable()
        {
            if(cogs.Count == 0)
                cogs = GetRestoredCogs().ToList();
            //ChainEvents.OnCogSetupRequest += CogsReady;
        }


        // private void CogsReady(CogHolder cogHolder)
        // {
        //     if (cogHolder != this)
        //     {
        //         print("not this");
        //         return;
        //     }
        //
        //     cogs.ForEach(c =>
        //     {
        //         c.Setup();
        //     });
        // }


        public Cogwheel[] GetRestoredCogs()
        {
            var _cogs = GetComponentsInChildren<Cogwheel>();
            for (var i = 0; i < _cogs.Length; i++)
            {
                SetCogComponentsId(i);
            }

            return _cogs;
        }

        void SetCogComponentsId(int i)
        {
            var components = cogs[i].GetComponentsInChildren<CogComponent>();
            foreach (var component in components)
            {
                component.Id = i;
            }
        }

        public Cogwheel[] AddCog(Cogwheel newCog)
        {
            cogs.Add(newCog);
            SetCogComponentsId(cogs.Count - 1);
            return cogs.ToArray();
        }

        public Cogwheel[] RemoveCog(Cogwheel cogToRemove)
        {
            cogs.Remove(cogToRemove);
            return cogs.ToArray();
        }

        public Cogwheel[] GetChainRelatedCogs()
        {
            return cogs.Where(c => c.Data.ContactType == ChainEnums.CogContactType.ChainRelated).ToArray();
        }

        public bool showGizmos = true;

        public void DrawGizmosOnSelectedCog(int i)
        {
            cogs.ForEach(c => c.drawGizmos = false);
            if (!showGizmos) return;
            cogs[i].drawGizmos = true;
        }

        private void OnDisable()
        {
            //ChainEvents.OnCogSetupRequest -= CogsReady;
        }
    }
}