namespace Actor
{
    public abstract class ActorController
    {
        protected ActorHolder Holder;
        public ActorController(ActorHolder holder)
        {
            Holder = holder;
        }

        public abstract void Subscribe();
        public abstract void Unsubscribe();

    }
}