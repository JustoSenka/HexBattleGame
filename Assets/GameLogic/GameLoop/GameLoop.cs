namespace Assets
{
    [RegisterDependency(typeof(IGameLoop), true)]
    public class GameLoop : IGameLoop
    {

        public GameLoop(IHexDatabase hexDatabase, 
            IUnitMovementManager unitMovementManager, 
            IUnitLifetimeManager unitLifetimeManager)
        {
        }

        public virtual void Awake()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {
            // Simulate user input here. Simulating SelectionManager should be sufficient
        }
    }
}
