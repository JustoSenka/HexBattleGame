using System;

namespace Assets
{
    [Serializable]
    public struct SkillEffect
    {
        public int ID;

        public int Amount;
        public bool ShouldSet;
        public bool IsPermament;
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
