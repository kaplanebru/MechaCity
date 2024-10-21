namespace Actor
{
    public abstract class ActorUnit
    {
        public ActorHolder Holder;
        public ActorUnit(ActorHolder holder)
        {
            Holder = holder;
        }

        public abstract void Subscribe();
        public abstract void Unsubscribe();

    }
}