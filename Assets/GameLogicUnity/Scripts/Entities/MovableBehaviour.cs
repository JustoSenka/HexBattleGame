using UnityEngine;

namespace Assets
{
    public class MovableBehaviour : SelectableBehaviour
    {
        public override int2 Cell { get { return Movable.Cell; } set { Movable.Cell = value; } }
        public override Selectable Selectable => Movable;
        public virtual Movable Movable => m_Movable;

        [SerializeField]
        private Movable m_Movable;
    }
}
