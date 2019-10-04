using UnityEngine;

namespace Assets
{
    public class SnapToGrid : MonoBehaviour
    {
        public HexCell HexCell;
        public bool SnapToGridOnStart;

        // Move this to system
        public virtual void Start()
        {
            HexCell = new HexCell(HexUtility.WorldPointToHex(transform.position, 1));

            if (SnapToGridOnStart)
                transform.position = HexCell.WorldPosition + new Vector3(0, transform.localScale.y / 2, 0);
        }
    }
}
