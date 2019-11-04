using System.Collections.Generic;

namespace Assets
{
    public interface IDatabase
    {
        void Start();

        Dictionary<string, Unit> Units { get; }
        Dictionary<string, Skill> Skills { get; }
        Dictionary<string, Item> Items { get; }
        Dictionary<string, Map> Maps { get; }
    }
}
