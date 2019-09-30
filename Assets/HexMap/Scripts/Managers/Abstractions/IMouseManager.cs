using System;

namespace Assets
{
    public interface IMouseManager
    {
        bool IsUnderCell { get; }
        HexCell HexUnderMouse { get; }

        bool IsUnderSelectable { get; }
        Selectable SelectableUnderMouse { get; }

        event Action<HexCell> HexSelected;
        event Action<HexCell> HexPressedDown;
        event Action<HexCell> HexUnselected;

        event Action<Selectable> SelectableSelected;
        event Action<Selectable> SelectableUnselected;

        event Action MouseReleased;

        void Update();
    }
}
