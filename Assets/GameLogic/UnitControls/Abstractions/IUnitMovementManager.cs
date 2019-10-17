using System;
using System.Collections.Generic;

namespace Assets
{
    public interface IUnitMovementManager
    {
        event Action<Action<Unit>, Unit, IEnumerable<int2>> UnitPositionChange;
        event Action<Unit> UnitPositionChangeEnd;
    }
}
