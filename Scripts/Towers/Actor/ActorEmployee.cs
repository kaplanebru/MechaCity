namespace Actor
{
    public abstract class ActorEmployee
    {
        public ActorHolder Holder;
        public ActorEmployee(ActorHolder holder)
        {
            Holder = holder;
        }

        public abstract void Subscribe();
        public abstract void Unsubscribe();

    }
}