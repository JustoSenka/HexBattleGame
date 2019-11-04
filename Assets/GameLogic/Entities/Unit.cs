using System;
using System.Collections.Generic;

namespace Assets
{
    [Serializable]
    public class Unit : Movable
    {
        public int ID;
        public int[] SkillIDs;

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

#if UNITY_EDITOR
        [NonSerialized]
        public SkillType SkillFlags; // Used only for editor so it's easy to setup skills for unit. Gameplay uses SkillIDs array and List<Skill>
#endif

        [NonSerialized]
        public List<Skill> AvailableSkills;

        public List<Skill> AffectedBy;
    }
}
