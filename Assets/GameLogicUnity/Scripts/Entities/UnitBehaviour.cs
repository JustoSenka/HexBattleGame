using UnityEngine;

namespace Assets
{
    public class UnitBehaviour : MovableBehaviour
    {
        public override int2 Cell { get { return m_Unit.Cell; } set { m_Unit.Cell = value; } }
        /*public override Selectable Selectable => Unit;
        public override Movable Movable => Unit;
        public virtual Unit Unit => m_Unit;*/

        [SerializeField]
        private Unit m_Unit;

        public float Height;
    }
}
