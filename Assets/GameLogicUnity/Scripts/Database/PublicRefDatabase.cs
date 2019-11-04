using System.Collections.Generic;

namespace Assets
{
    [RegisterDependency(typeof(IDatabase), true)]
    public class PublicRefDatabase : IDatabase
    {
        public Dictionary<string, Unit> Units { get; private set; }
        public Dictionary<string, Skill> Skills { get; private set; }
        public Dictionary<string, Item> Items { get; private set; }
        public Dictionary<string, Map> Maps { get; private set; }

        [Dependency(typeof(PublicReferences))]
        public PublicReferences PublicReferences;

        public PublicRefDatabase()
        {
            Units = new Dictionary<string, Unit>();
            Skills = new Dictionary<string, Skill>();
            Items = new Dictionary<string, Item>();
            Maps = new Dictionary<string, Map>();
        }

        public void Start()
        {
            foreach (var unit in PublicReferences.UnitDB.Units)
                Units.Add(unit.Name, unit);

            foreach (var skill in PublicReferences.SkillDB.Skills)
                Skills.Add(skill.Name, skill);

            foreach (var map in PublicReferences.MapDB)
                Maps.Add(map.Map.Name, map.Map);
        }
    }
}
