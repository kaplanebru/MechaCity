using System.Collections.Generic;
using System.Linq;

namespace Health
{
    public class ActorData
    {
        public int Health;
        public int InitialHealth;
        public List<int> LinkedActors;
        
        public ActorData(int initialHealth, List<int> linkedActors)
        {
            Health = initialHealth;
            InitialHealth = initialHealth;
            LinkedActors = linkedActors;
        }

        public void SetInitialHealth(int initialHealth)
        {
            Health = initialHealth;
            InitialHealth = initialHealth;
        }

        public void SetLinkedTowers(params int[] linkedTowers)
        {
            LinkedActors = linkedTowers.ToList();
        }
    }
}