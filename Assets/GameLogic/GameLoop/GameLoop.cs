namespace Assets
{
    [RegisterDependency(typeof(IGameLoop), true)]
    public class GameLoop : IGameLoop
    {
        private readonly IHexDatabase HexDatabase;
        private readonly IUnitMovementManager UnitMovementManager;

        public GameLoop(IHexDatabase hexDatabase, IUnitMovementManager unitMovementManager)
        {
            HexDatabase = hexDatabase;
            UnitMovementManager = unitMovementManager;
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
