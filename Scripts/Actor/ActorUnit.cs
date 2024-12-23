namespace Actor
{
    public abstract class ActorUnit
    {
        public ActorDB DB;
        public ActorUnit(ActorDB db)
        {
            DB = db;
        }

        public abstract void Subscribe();
        public abstract void Unsubscribe();

    }
}