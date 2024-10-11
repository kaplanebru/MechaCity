namespace Actor
{
    public abstract class ActorController
    {
        protected ActorManager _manager;
        public ActorController(ActorManager manager)
        {
            _manager = manager;
        }

        public abstract void Subscribe();
        public abstract void Unsubscribe();

    }
}