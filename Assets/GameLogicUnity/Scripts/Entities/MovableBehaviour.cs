using UnityEngine;

namespace Assets
{
    public class MovableBehaviour : SelectableBehaviour
    {
        public override int2 Cell { get { return m_Movable.Cell; } set { m_Movable.Cell = value; } }
        /*public override Selectable Selectable => Movable;
        public virtual Movable Movable => m_Movable;*/

        [SerializeField]
        private Movable m_Movable;
    }
}
