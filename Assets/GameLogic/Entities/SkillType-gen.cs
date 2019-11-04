
// Generated file by: Tools/Build/Skill Enum
// Maps enum for highlighters to correct material index in PublicReferences component in Managers scene

using System;
using System.Collections.Generic;

namespace Assets
{
    [Flags]
    public enum SkillType
    {
        None = 0,
        Attack = 1,
        Guard = 1 << 1,
        Heal = 1 << 2,

    }

    public static class SkillTypeIDs
    {
        public static Dictionary<int, SkillType> IdToSkillType = new Dictionary<int, SkillType>
        {
            { 0, SkillType.None },
            { 1, SkillType.Attack },
            { 2, SkillType.Guard },
            { 3, SkillType.Heal },

        };

        public static Dictionary<SkillType, int> SkillTypeToId = new Dictionary<SkillType, int>
        {
            { SkillType.None, 0 },
            { SkillType.Attack, 1 },
            { SkillType.Guard, 2 },
            { SkillType.Heal, 3 },

        };
    }
}