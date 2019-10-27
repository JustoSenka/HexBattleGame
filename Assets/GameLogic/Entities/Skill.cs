using System;
using System.Collections.Generic;

namespace Assets
{
    [Serializable]
    public struct Skill
    {
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
}
