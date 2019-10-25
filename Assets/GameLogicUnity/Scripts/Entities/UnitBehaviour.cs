using UnityEngine;

namespace Assets
{
    public class UnitBehaviour : MovableBehaviour
    {
        public override int2 Cell { get { return m_Unit.Cell; } set { m_Unit.Cell = value; } }
        /*public override Selectable Selectable => Unit;
        public override Movable Movable => Unit;
        public virtual Unit Unit => m_Unit;*/

#pragma warning disable CS0649
        [SerializeField]
        private Unit m_Unit;
#pragma warning restore CS0649

        public float Height;
    }
}
