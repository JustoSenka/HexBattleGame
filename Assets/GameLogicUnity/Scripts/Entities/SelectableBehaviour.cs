using UnityEngine;

namespace Assets
{
    public class SelectableBehaviour : SnapToGrid
    {
        public override int2 Cell { get { return Selectable.Cell; } set { Selectable.Cell = value; } }
        public virtual Selectable Selectable => m_Selectable;

        [SerializeField]
        private Selectable m_Selectable;
    }
}
