using System;

namespace Assets
{
    public interface IUnitMovementManager
    {
        PathfinderDictionary Paths { get; }

        event Action<Unit> UnitSelected;
        event Action<Unit> UnitUnselected;

        void Update();
    }
}
