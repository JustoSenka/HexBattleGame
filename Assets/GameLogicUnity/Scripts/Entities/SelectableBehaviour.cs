using UnityEngine;

namespace Assets
{
    public class SelectableBehaviour : SnapToGrid
    {
        public override int2 Cell { get { return m_Selectable.Cell; } set { m_Selectable.Cell = value; } }
        //public virtual Selectable Selectable => m_Selectable;

#pragma warning disable CS0649
        [SerializeField]
        private Selectable m_Selectable;
#pragma warning restore CS0649
    }
}
