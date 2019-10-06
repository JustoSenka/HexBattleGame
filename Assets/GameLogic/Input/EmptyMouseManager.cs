using System;

namespace Assets
{
    public class MouseManager : IMouseManager
    {
        public bool IsUnderCell => default;
        public HexCell HexUnderMouse => default;
        public bool IsUnderSelectable => default;
        public Selectable SelectableUnderMouse => default;

        public event Action<HexCell> HexSelected;
        public event Action<HexCell> HexPressedDown;
        public event Action<HexCell> HexUnselected;
        public event Action<HexCell> HexUnderMouseChanged;

        public event Action<Selectable> SelectableSelected;
        public event Action<Selectable> SelectableUnselected;
        public event Action<Selectable> SelectableUnderMouseChanged;

        public event Action MouseReleased;

        void IMouseManager.Update()
        {
            
        }
    }
}
