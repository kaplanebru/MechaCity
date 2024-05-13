using System.Collections;
using Chain;
using UnityEngine;

namespace ChainInGame
{
    public class MachineryInGame
    {
        private Machinery _machinery;
        private CogHolder _cogHolder;
        public MachineryInGame(Machinery machinery)
        {
            _machinery = machinery;
            _cogHolder = machinery.cogHolder;

            if (_machinery.chainGenerator.ChainData == null)
            {
                Debug.Log(machinery.name+" is not Chain Related");
            }
        }
        
        public void AddToMachinery(Cogwheel _gear)
        {
            var gear = _gear;
            if(_cogHolder.cogs.Contains(gear)) return;
            
            gear.transform.SetParent(_cogHolder.transform);
            _cogHolder.cogs.Add(gear);

            Regenerate();
        }

        public void RemoveFromMachinery(Cogwheel _gear)
        {
            var gear = _gear;
            
            gear.transform.SetParent(null);
            _cogHolder.cogs.Remove(gear);
            
            Regenerate();
        }

        public void EmptyMachinery()
        {
            for (var i = _cogHolder.cogs.Count - 1; i >= 0; i--)
            {
                var gear = _cogHolder.cogs[i];
                gear.transform.SetParent(gear.parent); //!!important
                _cogHolder.cogs.Remove(gear); 
            }

            Regenerate();
            //gear'ın parenti meselesi ve id'sini ayrı scriptte verebiliriz
        }
        
        void GenerateChain()
        {
            _machinery.chainGenerator.GenerateChain(null, _machinery.cogHolder.GetChainRelatedCogs());
        }
        
        public void GenerateAndMove()
        {
            GenerateChain();
            _machinery.SetMovers();
            _machinery.Move();
        }
        
        public void StopMotion()
        {
            _machinery.StopMovers();
        }

        public void StartMotion()
        {
            _machinery.Move();
        }

        public void ResetChain()
        {
            if(_machinery.chainGenerator.ChainData != null)
                _machinery.chainGenerator.ResetLinks();
        }

        public void Regenerate()
        {
            StopMotion();
            ResetChain();

            GenerateChain();
            _machinery.SetMovers();
        }
    }
}