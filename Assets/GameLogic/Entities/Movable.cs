using System;

namespace Assets
{
    [Serializable]
    public class Movable : Selectable
    {
        public int Movement;
        public int MaxMovement;
    }
}
