using System;

namespace Assets
{
    public interface IMouseManager
    {
        bool IsUnderCell { get; }
        HexCell HexUnderMouse { get; }

        bool IsUnderSelectable { get; }
        Selectable SelectableUnderMouse { get; }

        event Action<HexCell> HexClicked;
        event Action<HexCell> HexSelected;
        event Action MouseReleased;
        event Action<Selectable> SelectableClicked;


        void Update();
    }
}
