using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Assets
{
    [RegisterDependency(typeof(IHexDatabase), true)]
    public class HexDatabase : IHexDatabase
    {
        public IDictionary<int2, HexCell> m_CellMap;

        [Dependency(typeof(PublicReferences))]
        private PublicReferences PublicReferences;

        public HexDatabase()
        {
            m_CellMap = new Dictionary<int2, HexCell>();
        }

        public void Start()
        {
            var allLoadedSceneNames = GetAllSceneNames().ToArray();
            foreach (var db in PublicReferences.MapHexDB.Where(d => allLoadedSceneNames.Contains(d.SceneName)))
            {
                var data = db.Data;
                foreach (var pair in data)
                {
                    var newCell = new HexCell(pair.Key);
                    newCell.Type = pair.Value;
                    m_CellMap[pair.Key] = newCell;
                }
            }
        }

        public HexCell GetCell(int2 pos)
        {
            if (!m_CellMap.TryGetValue(pos, out HexCell hexCell))
            {
                hexCell = new HexCell(pos);
                m_CellMap[pos] = hexCell;
            }

            return hexCell;
        }

        public void UpdateCell(HexCell hex) => m_CellMap[hex.Position] = hex;

        // Private ---

        private static IEnumerable<string> GetAllSceneNames()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
                yield return SceneManager.GetSceneAt(i).name;
        }
    }
}
