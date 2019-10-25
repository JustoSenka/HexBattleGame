using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    [RegisterDependency(typeof(HexDebugger), true)]
    public class HexDebugger
    {
        [Dependency(typeof(PublicReferences))]
        public PublicReferences PublicReferences;

        private readonly IMouseInputManager UserInputManager;
        private readonly IHexHighlighter HexHighlighter;

        private ObjectPool m_Pool;
        private PoolItem[] m_Items;
        private HexCell m_HoveringCell;

        public HexDebugger(IMouseInputManager UserInputManager, IHexHighlighter HexHighlighter)
        {
            this.UserInputManager = UserInputManager;
            this.HexHighlighter = HexHighlighter;
        }

        public void Start()
        {
            if (PublicReferences == null)
            {
                Debug.LogWarning(this.GetType() + ": PublicReferences == null");
                return;
            }

            var parent = new GameObject("Debug Text").transform;
            m_Pool = new ObjectPool(parent.gameObject, PublicReferences.DebugCellIndexText, 0);

            for (int i = -20; i <= 20; i++)
            {
                for (int j = -20; j <= 20; j++)
                {
                    var pos = new int2(i, j);
                    var hex = new HexCell(pos);

                    var go = GameObject.Instantiate(PublicReferences.DebugCellIndexText,
                        hex.WorldPosition + new Vector3(0, 0.1f, 0),
                        PublicReferences.DebugCellIndexText.transform.rotation, parent);

                    go.GetComponentInChildren<Text>().text = pos.x + "." + pos.y;
                }
            }

        }

        public void Update()
        {
            /*if (MouseManager.IsUnderCell)
            {
                if (m_HoveringCell != MouseManager.HexUnderMouse)
                {
                    var cells = HexUtility.FindNeighbours(MouseManager.HexUnderMouse.Position, 10).Select(c => new HexCell(c));
                    if (m_Items == null)
                        m_Items = m_Pool.ReserveItems(cells.Count());

                    // for each item assing one cell from the list
                    foreach (var (item, cell) in m_Items.Zip(cells, (item, cell) => (item, cell)))
                    {
                        item.GameObject.transform.position = cell.WorldPosition + new Vector3(0, 0.1f, 0);
                        item.GameObject.transform.rotation = PublicReferences.DebugCellIndexText.transform.rotation;
                        item.GameObject.GetComponentInChildren<Text>().text = cell.Position.x + "." + cell.Position.y;
                    }
                }
            }
            else
            {
                m_Items.Release();
                m_Items = null;
            }

            m_HoveringCell = MouseManager.HexUnderMouse;*/
        }
    }
}
