using UnityEngine;

namespace Assets
{
    public class Selectable : MonoBehaviour
    {
        public int Team;
        public HexCell HexCell;

        // Move this to system
        public void Start()
        {
            HexCell = new HexCell(HexCell.Position); // HexCell created from inspector will be invalid and have default values
            transform.position = HexCell.WorldPosition + new Vector3(0, transform.localScale.y / 2, 0);
        }
    }
}
