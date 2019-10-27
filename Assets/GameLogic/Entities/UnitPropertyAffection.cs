using System;

namespace Assets
{
    [Serializable]
    public struct SkillEffect
    {
        public int Amount;
        public bool ShouldSet;
        public PropertyName PropertyName;
    }

    public enum PropertyName
    {
        Attack,
        Defense,

        Health,
        Magic,

        Range,
        Movement,
    }
}
