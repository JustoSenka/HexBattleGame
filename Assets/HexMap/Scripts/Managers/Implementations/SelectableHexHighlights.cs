namespace Assets
{
    [RegisterDependency(typeof(ISelectableHexHighlights), true)]
    public class SelectableHexHighlights : ISelectableHexHighlights
    {
        private readonly IMouseManager MouseManager;
        private readonly IHexHighlighter HexHighlighter;

        private PoolItem m_TempSelectItemForMapDragging;
        private PoolItem m_SelectItem;

        private PoolItem m_HoverItem;
        private PoolItem m_RedItem;

        public SelectableHexHighlights(IMouseManager MouseManager, IHexHighlighter HexHighlighter)
        {
            this.MouseManager = MouseManager;
            this.HexHighlighter = HexHighlighter;

            MouseManager.HexSelected += OnHexSelected;
            MouseManager.HexUnselected += OnHexUnselected;
            MouseManager.HexPressedDown += OnHexPressedDown;

            MouseManager.SelectableSelected += SelectableSelected;
            MouseManager.SelectableUnselected += OnSelectableUnselected;
            MouseManager.MouseReleased += OnMouseReleased;
        }

        public void Start() { }

        public void Update()
        {
            if (MouseManager.IsUnderCell)
                m_HoverItem = HexHighlighter.PlaceHighlighter(MouseManager.HexUnderMouse, Highlighter.Hover, m_HoverItem);
        }

        private void OnHexSelected(HexCell hex)
        {
            m_RedItem = HexHighlighter.PlaceHighlighter(hex, Highlighter.Red, m_RedItem);
        }

        private void OnHexUnselected(HexCell hex)
        {
            m_RedItem?.Release();
            m_RedItem = null;
        }

        private void OnHexPressedDown(HexCell hex)
        {
            m_TempSelectItemForMapDragging = HexHighlighter.PlaceHighlighter(hex, Highlighter.Select, m_TempSelectItemForMapDragging);
        }

        private void SelectableSelected(Selectable obj)
        {
            m_SelectItem = HexHighlighter.PlaceHighlighter(obj.HexCell, Highlighter.Select, m_SelectItem);
        }

        private void OnSelectableUnselected(Selectable obj)
        {
            m_SelectItem?.Release();
            m_SelectItem = null;
        }

        private void OnMouseReleased()
        {
            m_TempSelectItemForMapDragging?.Release();
            m_TempSelectItemForMapDragging = null;
        }
    }
}
