namespace Assets
{
    public static class TestUtility
    {
        public static DependencyContainer CreateContainer()
        {
            var c = new DependencyContainer();
            c.PopulateDependenciesFromAttributesInDomain();
            return c;
        }
    }
}
