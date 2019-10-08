namespace Assets
{
    [RegisterDependency(typeof(ISelectableHexHighlights), true)]
    public class SelectableHexHighlights : ISelectableHexHighlights
    {
        private PoolItem m_TempSelectItemForMapDragging;
        private PoolItem m_SelectItem;

        private PoolItem m_HoverItem;
        private PoolItem m_RedItem;

        private readonly IUserInputManager UserInputManager;
        private readonly IHexHighlighter HexHighlighter;
        private readonly ISelectionManager SelectionManager;
        public SelectableHexHighlights(IUserInputManager UserInputManager, ISelectionManager SelectionManager, IHexHighlighter HexHighlighter)
        {
            this.UserInputManager = UserInputManager;
            this.HexHighlighter = HexHighlighter;
            this.SelectionManager = SelectionManager;

            SelectionManager.HexSelected += OnHexSelected;
            SelectionManager.HexUnselected += OnHexUnselected;

            SelectionManager.SelectableSelected += SelectableSelected;
            SelectionManager.SelectableUnselected += OnSelectableUnselected;

            UserInputManager.HexPressedDown += OnHexPressedDown;
            UserInputManager.MouseReleased += OnMouseReleased;

            UserInputManager.HexUnderMouseChanged += OnHexUnderMouseChanged;
        }

        private void OnHexUnderMouseChanged(HexCell hexCell)
        {
            if (hexCell.IsValid)
                m_HoverItem = HexHighlighter.PlaceHighlighter(hexCell, Highlighter.Hover, m_HoverItem);
            else
            {
                m_HoverItem.Release();
                m_HoverItem = null;
            }
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
            m_SelectItem = HexHighlighter.PlaceHighlighter(new HexCell(obj.Cell), Highlighter.Select, m_SelectItem);
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
