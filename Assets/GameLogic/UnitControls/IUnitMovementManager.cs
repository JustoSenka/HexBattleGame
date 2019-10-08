using System;
using System.Collections.Generic;

namespace Assets
{
    public interface IUnitMovementManager
    {
        PathfinderDictionary Paths { get; }

        event Action<Unit> UnitSelected;
        event Action<Unit> UnitUnselected;

        event Action<Action, Unit, IEnumerable<int2>> UnitPositionChange;
    }
}
