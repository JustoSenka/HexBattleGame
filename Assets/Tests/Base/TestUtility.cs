namespace Assets
{
    public static class TestUtility
    {
        public static DependencyContainer CreateContainer()
        {
            var c = new DependencyContainer();
            c.PopulateDependenciesFromAttributesInAssembly(typeof(GameLoop).Assembly);
            c.PopulateDependenciesFromAttributesInAssembly(typeof(TestUtility).Assembly);

            c.Resolve<ICrossPlayerController>().LocalTeam = 0;

            c.InstantiateSingletons();
            return c;
        }

        public static Unit CreateUnit(int2 pos = default, int team = 0)
        {
            return new Unit()
            {
                Attack = 5,
                Defense = 1,
                Health = 20,
                Magic = 5,
                MaxHealth = 20,
                MaxMagic = 5,
                MaxMovement = 5,
                Movement = 5,
                RangeMax = 1,
                RangeMin = 1,
                Team = team,
                Cell = pos,
            };
        }
    }
}
