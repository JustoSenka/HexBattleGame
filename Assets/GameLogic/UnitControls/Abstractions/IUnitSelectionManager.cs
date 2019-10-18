using System;

namespace Assets
{
    public interface IUnitSelectionManager
    {
        bool CanSelectedUnitPerformActions { get; }

        PathfinderDictionary Paths { get; }
        Unit SelectedUnit { get; }

        event Action<Unit> UnitSelected;
        event Action<Unit> UnitUnselected;

        bool CanLocalPlayerControlThisUnit(Selectable unit);
        PathfinderDictionary FindAllPathsWhereUnitCanMove(Unit unit);
    }
}
