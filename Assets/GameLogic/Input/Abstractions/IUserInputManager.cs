using System;

namespace Assets
{
    public interface IUserInputManager
    {
        bool IsUnderCell { get; }
        HexCell HexUnderMouse { get; }

        bool IsUnderSelectable { get; }
        Selectable SelectableUnderMouse { get; }

        event Action<HexCell> HexPressedDown;
        event Action MouseReleased;

        event Action<HexCell> HexUnderMouseChanged;
        event Action<Selectable> SelectableUnderMouseChanged;

        void Update();
    }
}
