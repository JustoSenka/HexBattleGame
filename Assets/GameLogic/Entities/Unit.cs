using System;

namespace Assets
{
    [Serializable]
    public class Unit : Movable
    {
        public int Attack;
        public int Defense;

        public int MaxHealth;
        public int Health;
        public int MaxMagic;
        public int Magic;

        public int Range;
    }
}
