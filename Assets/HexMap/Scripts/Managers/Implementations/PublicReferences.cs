using UnityEngine;

namespace Assets
{
    public class PublicReferences : MonoBehaviour
    {
        [Space(10)]
        [Header("Map Hex Databases")]
        public HexDatabaseData[] MapHexDB;

        [Space(10)]
        [Header("Prefabs")]
        public GameObject BlueHighlightPrefab;
        public GameObject RedHighlightPrefab;
        public GameObject HoverHighlightPrefab;
        public GameObject SelectionHighlightPrefab;

        [Space(10)]
        [Header("Debug")]
        public GameObject DebugCellIndexText;
    }
}
