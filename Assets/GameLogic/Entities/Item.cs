using System;

namespace Assets
{
    [Serializable]
    public struct Item
    {
        public ItemClass Class;
        public string Name;

        public int Price;

        public SkillEffect[] Effect;
    }

    public enum ItemClass
    {
        Weapon, Armour, Shield, Helmet, Talisman
    }
}
