using System.Linq;

namespace Assets
{
    [RegisterDependency(typeof(IHexHighlightManager), true)]
    public class HexHighlightManager : IHexHighlightManager
    {
        [Dependency(typeof(IMouseManager))]
        public IMouseManager MouseManager;

        [Dependency(typeof(PublicReferences))]
        public PublicReferences PrefabReferences;

        private ObjectPool m_PoolBlue;
        private ObjectPool m_PoolRed;
        private ObjectPool m_PoolSelection;
        private ObjectPool m_PoolHover;

        private PoolItem m_HoverItem;
        private PoolItem m_SelectionItem;

        public void Start()
        {
            m_PoolBlue = new ObjectPool(PrefabReferences.GeneratedPoolObjects, PrefabReferences.BlueHighlightPrefab, 0);
            m_PoolRed = new ObjectPool(PrefabReferences.GeneratedPoolObjects, PrefabReferences.RedHighlightPrefab, 0);
            m_PoolSelection = new ObjectPool(PrefabReferences.GeneratedPoolObjects, PrefabReferences.SelectionHighlightPrefab, 0);
            m_PoolHover = new ObjectPool(PrefabReferences.GeneratedPoolObjects, PrefabReferences.HoverHighlightPrefab, 0);

            MouseManager.HexClicked += OnHexClicked;
            MouseManager.HexSelected += OnHexSelected;
            MouseManager.MouseReleased += OnMouseReleased;
        }

        public void Update()
        {
            // Highlights objects under mouse

            if (MouseManager.IsUnderCell)
            {
                if (m_HoverItem == null)
                    m_HoverItem = m_PoolHover.ReserveItems(1).First();

                m_HoverItem.GameObject.transform.position = MouseManager.HexUnderMouse.WorldPosition;
            }
            else
            {
                if (m_HoverItem != null)
                {
                    m_HoverItem.Release();
                    m_HoverItem = null;
                }
            }
        }

        private void OnHexClicked(HexCell hex)
        {
            var item = m_PoolRed.ReserveItems(1).First();
            item.GameObject.transform.position = hex.WorldPosition;
        }

        private void OnHexSelected(HexCell hex)
        {
            if (m_SelectionItem == null)
                m_SelectionItem = m_PoolSelection.ReserveItems(1).First();

            m_SelectionItem.GameObject.transform.position = hex.WorldPosition;
        }

        private void OnMouseReleased()
        {
            if (m_SelectionItem != null)
            {
                m_SelectionItem.Release();
                m_SelectionItem = null;
            }
        }
    }
}
