using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    [Serializable]
    public struct Skill
    {
        public int ID;
        public string Name;

        public int MagicReq;

        public int Cooldown;
        public int CooldownLeft;

        public int Duration;
        public int DurationLeft;

        public int AOE;

        public int RangeMin;
        public int RangeMax;

        public List<SkillEffect> Effects;
    }

#if UNITY_EDITOR
    public static class SkillTypeExtension
    {
        public static SkillType SkillTypeFromIDs(params int[] ids)
        {
            if (ids == null || ids.Length == 0)
                return default;

            return ids.Select(id => SkillTypeIDs.IdToSkillType[id]).Aggregate((a, b) => a | b);
        }

        public static int[] IDsFromSkillType(this SkillType skillType)
        {
            return Enum.GetValues(typeof(SkillType)).Cast<SkillType>()
                .Where(f => skillType.HasFlag(f))
                .Select(s => SkillTypeIDs.SkillTypeToId[s])
                .ToArray();
        }
    }
#endif
}
