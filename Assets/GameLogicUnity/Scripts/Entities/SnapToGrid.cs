using System;
using UnityEngine;

namespace Assets
{
    [Serializable]
    public class SnapToGrid : MonoBehaviour
    {
        public virtual int2 Cell { get; set; }

        public bool SnapToGridOnStart;

        public virtual void Start()
        {
            var HexCell = new HexCell(HexUtility.WorldPointToHex(transform.position, 1));
            Cell = HexCell.Position;
            if (SnapToGridOnStart)
                transform.position = HexCell.WorldPosition + new Vector3(0, transform.localScale.y / 2, 0);
        }
    }
}
