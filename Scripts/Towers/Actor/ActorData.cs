using System.Collections.Generic;

namespace Health
{
    public class ActorData
    {
        public int Health;
        public int InitialHealth;
        public List<int> LinkedTowers;
        
        public ActorData(int initialHealth, List<int> linkedTowers = null)
        {
            Health = initialHealth;
            InitialHealth = initialHealth;
            LinkedTowers = linkedTowers;
        }
    }
}