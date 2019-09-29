using System;

namespace Assets
{
    public interface IMouseManager
    {
        bool IsUnderCell { get; }
        HexCell HexUnderMouse { get; }

        event Action<HexCell> HexClicked;
        event Action<HexCell> HexSelected;
        event Action MouseReleased;

        void Update();
    }
}
