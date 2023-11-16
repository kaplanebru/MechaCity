using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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

        public void DisableAllGizmos()
        {
            cogs.ForEach(c => c.drawGizmos = false);
        }
        public void DrawGizmosOnSelectedCog(int i)
        {
            if (!showGizmos) return;
            
            DisableAllGizmos();
            cogs[i].drawGizmos = true;
        }
        
        public Vector3 NewCogPos(float radius)
        {
            Vector3 newPos;

            if (cogs.Count == 0) return Vector3.zero;

            var cogPositions = new Vector3[cogs.Count];
            for (int i = 0; i < cogs.Count; i++)
            {
                cogPositions[i] = cogs[i].transform.localPosition;
            }

            Vector3 center = TrigonometryHelper.Center(cogPositions);
            var outermostCog = cogs.OrderByDescending(c => Vector3.Distance(center, c.transform.localPosition)).First();
      
            float distanceFromCenter =
                Vector3.Distance(center, outermostCog.transform.localPosition); // + outermostCog.Data.Radius;


            CreatePos:
            newPos = TrigonometryHelper.CirclePoint(UnityEngine.Random.Range(0, 360), distanceFromCenter) + center;
            float offset = radius * 2 + 2;
            newPos += new Vector3(offset, 0, offset); //not: hiç offset olmazsa sonsuz döngüye girebiliyor

            foreach (var cog in cogs)
            {
                if (Vector3.Distance(newPos, cog.transform.localPosition) <= cog.Data.Radius + radius)
                    goto CreatePos;
            }
            
            return newPos;
        }


        private void OnDisable()
        {
            //ChainEvents.OnCogSetupRequest -= CogsReady;
        }
    }
}