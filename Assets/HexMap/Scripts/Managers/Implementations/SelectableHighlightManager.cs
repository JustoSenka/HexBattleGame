using System;
using System.Linq;

namespace Assets
{
    [RegisterDependency(typeof(ISelectableHighlightManager), true)]
    public class SelectableHighlightManager : ISelectableHighlightManager
    {
        private readonly IMouseManager MouseManager;
        private readonly IHexHighlighter HexHighlighter;

        private Selectable m_CurrentlySelectedObject;

        private PoolItem m_HoverItem;
        private PoolItem m_SelectItem;
        private PoolItem m_RedItem;

        public SelectableHighlightManager(IMouseManager MouseManager, IHexHighlighter HexHighlighter)
        {
            this.MouseManager = MouseManager;
            this.HexHighlighter = HexHighlighter;

            MouseManager.HexClicked += OnHexClicked;
            MouseManager.HexSelected += OnHexSelected;
            MouseManager.MouseReleased += OnMouseReleased;
            MouseManager.SelectableClicked += OnSelectableClicked;
        }

        public void Start()
        {
        }

        public void Update()
        {
            if (MouseManager.IsUnderCell)
                m_HoverItem = HexHighlighter.PlaceHighlighter(MouseManager.HexUnderMouse, Highlighter.Hover, m_HoverItem);
        }

        private void OnSelectableClicked(Selectable obj)
        {
            m_CurrentlySelectedObject = obj;
            m_SelectItem = HexHighlighter.PlaceHighlighter(MouseManager.HexUnderMouse, Highlighter.Select, m_SelectItem);
        }

        private void OnHexClicked(HexCell hex)
        {
            m_RedItem = HexHighlighter.PlaceHighlighter(MouseManager.HexUnderMouse, Highlighter.Red, m_RedItem);
        }

        private void OnHexSelected(HexCell hex)
        {
            m_SelectItem = HexHighlighter.PlaceHighlighter(MouseManager.HexUnderMouse, Highlighter.Select, m_SelectItem);
        }

        private void OnMouseReleased()
        {
            m_CurrentlySelectedObject = null;
            m_SelectItem.Release();
        }
    }
}
