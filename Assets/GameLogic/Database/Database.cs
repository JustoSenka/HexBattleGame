﻿using System.Collections.Generic;

namespace Assets
{
    [RegisterDependency(typeof(IDatabase), true)]
    public class Database : IDatabase
    {
        public Dictionary<string, Unit> Units { get; private set; }
        public Dictionary<string, Skill> Skills { get; private set; }
        public Dictionary<string, Item> Items { get; private set; }
        public Dictionary<string, Map> Maps { get; private set; }

        public Database()
        {
            Units = new Dictionary<string, Unit>();
            Skills = new Dictionary<string, Skill>();
            Items = new Dictionary<string, Item>();
            Maps = new Dictionary<string, Map>();
        }

        public void Start()
        {
            

        }
    }
}
