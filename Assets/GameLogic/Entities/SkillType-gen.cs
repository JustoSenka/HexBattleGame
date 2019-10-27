
// Generated file by: Tools/Build/Skill Enum
// Maps enum for highlighters to correct material index in PublicReferences component in Managers scene

using System;

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
}