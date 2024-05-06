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
        
        public void AddToMachinery(Interactable interactable)
        {
            var gear = interactable._gear;
            if(_cogHolder.cogs.Contains(gear)) return;
            
            gear.transform.SetParent(_cogHolder.transform);
            _cogHolder.cogs.Add(gear);

            Regenerate();
        }

        public void RemoveFromMachinery(Interactable interactable)
        {
            var gear = interactable._gear;
            
            gear.transform.SetParent(null);
            _cogHolder.cogs.Remove(gear);
            
            Regenerate();
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
           
            if (_machinery.cogHolder.cogs.Count > 1)
            {
                GenerateAndMove();
            }
        }
    }
}