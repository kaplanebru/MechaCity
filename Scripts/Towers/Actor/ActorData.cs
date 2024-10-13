using System.Collections.Generic;
using System.Linq;
using Enums;

namespace Actor
{
    public class ActorData
    {
        public uint ID;
        public ActorType Type;
        public int[] Towers;
        
        public int Health;
        public int InitialHealth;
        
        public List<uint> LinkedActors = new();
        
        public ActorData(uint id, ActorType type, int initialHealth, params int[] towers)
        {
            ID = id;
            Type = type;
            
            InitialHealth = initialHealth;
            Health = initialHealth;
            
            Towers = towers;
        }

        public void SetLinkedTowers(params uint[] linkedActors)
        {
            LinkedActors = linkedActors.ToList();
        }
        // public void SetInitialHealth(int initialHealth)
        // {
        //     Health = initialHealth;
        //     InitialHealth = initialHealth;
        // }

       
    }
}