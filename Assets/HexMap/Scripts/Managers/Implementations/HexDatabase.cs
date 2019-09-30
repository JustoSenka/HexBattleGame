using System.Collections.Generic;

namespace Assets
{
    [RegisterDependency(typeof(IHexDatabase), true)]
    public class HexDatabase : IHexDatabase
    {
        public IDictionary<int2, HexCell> m_CellMap;

        public HexDatabase()
        {
            m_CellMap = new Dictionary<int2, HexCell>();

            var start = -50;
            var end = 50;
            
            for (int i = start; i <= end; i++)
            {
                for (int j = start; j <= end; j++)
                {
                    var pos = new int2(i, j);
                    m_CellMap.Add(pos, new HexCell(pos));
                }
            }
        }

        public HexCell GetCell(int2 pos) => m_CellMap[pos];
        public void UpdateCell(HexCell hex) => m_CellMap[hex.Position] = hex;
    }
}
