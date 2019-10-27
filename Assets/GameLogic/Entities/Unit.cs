using System;
using System.Collections.Generic;

namespace Assets
{
    [Serializable]
    public class Unit : Movable
    {
        public string Name;
        public int Tier;

        public int Attack;
        public int Defense;

        public int MaxHealth;
        public int Health;
        public int MaxMagic;
        public int Magic;

        public int RangeMin;
        public int RangeMax;

        public List<Skill> AvailableSkills;
        public List<Skill> AffectedBy;
    }
}
