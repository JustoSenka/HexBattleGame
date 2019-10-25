using UnityEngine;

namespace Assets
{
    public class MovableBehaviour : SelectableBehaviour
    {
        public override int2 Cell { get { return m_Movable.Cell; } set { m_Movable.Cell = value; } }
        /*public override Selectable Selectable => Movable;
        public virtual Movable Movable => m_Movable;*/

#pragma warning disable CS0649
        [SerializeField]
        private Movable m_Movable;
#pragma warning restore CS0649
    }
}
