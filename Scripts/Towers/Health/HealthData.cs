namespace Health
{
    public class HealthData
    {
        public int Health;
        public int InitialHealth;

        public HealthData( int initialHealth)
        {
            Health = initialHealth;
            InitialHealth = initialHealth;
        }
    }
}