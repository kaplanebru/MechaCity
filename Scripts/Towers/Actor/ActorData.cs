using System.Collections.Generic;
using System.Linq;
using Enums;

namespace Health
{
    public class ActorData
    {
        public string ID;
        public ActorType Type;
        public List<int> Towers;
        
        public int Health;
        public int InitialHealth;
        
        public List<int> LinkedTowers;
        
        public ActorData(string id, ActorType type, int initialHealth, params int[] towers)
        {
            ID = id;
            Type = type;
            
            InitialHealth = initialHealth;
            Health = initialHealth;
            
            Towers = towers.ToList();
        }

        public void SetInitialHealth(int initialHealth)
        {
            Health = initialHealth;
            InitialHealth = initialHealth;
        }

        public void SetLinkedTowers(params int[] linkedTowers)
        {
            LinkedTowers = linkedTowers.ToList();
        }
    }
}