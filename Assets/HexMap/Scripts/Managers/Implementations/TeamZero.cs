namespace Assets
{
    [RegisterDependency(typeof(ITeam), true)]
    public class TeamZero : ITeam
    {
        public int TeamID => 0;
    }
}
