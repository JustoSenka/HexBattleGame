using UnityEngine;

namespace Assets
{
    public class PublicReferences : MonoBehaviour
    {
        [Space(10)]
        [Header("Map Hex Databases")]
        public MapDatabaseData[] MapDB;

        [Space(10)]
        [Header("Databases")]
        public UnitDatabaseData UnitDB;
        public SkillDatabaseData SkillDB;

        [Space(10)]
        [Header("Prefabs")]
        public GameObject HighlightPrefab;

        [Space(10)]
        [Header("Materials")]
        public Material[] HighlightMaterials;

        [Space(10)]
        [Header("Debug")]
        public GameObject DebugCellIndexText;
    }
}
