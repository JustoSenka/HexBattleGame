namespace Assets
{
    [RegisterDependency(typeof(ISelectableHexHighlights), true)]
    public class SelectableHexHighlights : ISelectableHexHighlights
    {
        private PoolItem m_TempSelectItemForMapDragging;
        private PoolItem m_SelectItem;

        private PoolItem m_HoverItem;
        private PoolItem m_HexSelectedItem;

        private readonly IMouseInputManager UserInputManager;
        private readonly IHexHighlighter HexHighlighter;
        private readonly ISelectionManager SelectionManager;
        private readonly ITurnManager TurnManager;
        public SelectableHexHighlights(IMouseInputManager UserInputManager, ISelectionManager SelectionManager, IHexHighlighter HexHighlighter, ITurnManager TurnManager)
        {
            this.UserInputManager = UserInputManager;
            this.HexHighlighter = HexHighlighter;
            this.SelectionManager = SelectionManager;
            this.TurnManager = TurnManager;

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
                m_HoverItem = HexHighlighter.PlaceHighlighter(hexCell, Highlighter.white_light, m_HoverItem);
            else
            {
                m_HoverItem.Release();
                m_HoverItem = null;
            }
        }

        private void OnHexSelected(HexCell hex)
        {
            m_HexSelectedItem = HexHighlighter.PlaceHighlighter(hex, Highlighter.grey, m_HexSelectedItem);
        }

        private void OnHexUnselected(HexCell hex)
        {
            m_HexSelectedItem?.Release();
            m_HexSelectedItem = null;
        }

        private void OnHexPressedDown(HexCell hex)
        {
            m_TempSelectItemForMapDragging = HexHighlighter.PlaceHighlighter(hex, Highlighter.white, m_TempSelectItemForMapDragging);
        }

        private void SelectableSelected(Selectable obj)
        {
            var highlighter = TurnManager.CurrentTurnOwner == obj ? Highlighter.white : Highlighter.grey;
            m_SelectItem = HexHighlighter.PlaceHighlighter(new HexCell(obj.Cell), highlighter, m_SelectItem);
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
