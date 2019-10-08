﻿using System;

namespace Assets
{
    public interface IUserInputManager
    {
        event Action<HexCell> HexPressedDown;
        event Action MouseReleased;

        event Action<HexCell> HexUnderMouseChanged;
        event Action<Selectable> SelectableUnderMouseChanged;

        void Update();
    }
}
