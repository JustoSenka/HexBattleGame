using System;

namespace Assets
{
    [RegisterDependency(typeof(IUnitSelectionManager), true)]
    public class UnitSelectionManager : IUnitSelectionManager
    {
        public PathfinderDictionary Paths { get; private set; }
        public Unit SelectedUnit { get; private set; }

        public bool CanSelectedUnitPerformActions => 
            SelectedUnit != null && 
            TurnManager.CurrentTurnOwner == SelectedUnit && 
            SelectedUnit.Team == CrossPlayerController.LocalTeam;

        public event Action<Unit> UnitSelected;
        public event Action<Unit> UnitUnselected;

        private readonly ISelectionManager SelectionManager;
        private readonly IHexPathfinder HexPathfinder;
        private readonly IHexDatabase HexDatabase;
        private readonly ITurnManager TurnManager;
        private readonly ICrossPlayerController CrossPlayerController;
        public UnitSelectionManager(ISelectionManager SelectionManager, IHexPathfinder HexPathfinder, IHexDatabase HexDatabase, ITurnManager TurnManager, ICrossPlayerController CrossPlayerController)
        {
            this.SelectionManager = SelectionManager;
            this.HexPathfinder = HexPathfinder;
            this.HexDatabase = HexDatabase;
            this.TurnManager = TurnManager;
            this.CrossPlayerController = CrossPlayerController;

            TurnManager.TurnStarted += OnTurnStarted;

            SelectionManager.SelectableSelected += OnSelectableSelected;
            SelectionManager.SelectableUnselected += OnSelectableUnselected;
        }

        public bool CanLocalPlayerControlThisUnit(Selectable unit)
        {
            return TurnManager.CurrentTurnOwner == unit && unit.Team == CrossPlayerController.LocalTeam;
        }

        public PathfinderDictionary FindAllPathsWhereUnitCanMove(Unit unit)
        {
            return HexPathfinder.FindAllPaths(unit.Cell, HexType.Empty, unit.Movement);
        }

        // Callbacks ---

        private void OnTurnStarted(Selectable obj)
        {
            if (CanLocalPlayerControlThisUnit(obj))
                SelectionManager.ClickAndSelectSelectable(obj);
        }

        private void OnSelectableSelected(Selectable obj)
        {
            if (obj is Unit unit)
                SelectUnit(unit);
        }

        private void OnSelectableUnselected(Selectable obj)
        {
            if (obj is Unit unit)
                UnselectUnit(unit);
        }

        // Private ---

        private void SelectUnit(Unit unit)
        {
            SelectedUnit = unit;
            Paths = FindAllPathsWhereUnitCanMove(unit);
            UnitSelected?.Invoke(unit);
        }

        private void UnselectUnit(Unit unit)
        {
            SelectedUnit = default;
            UnitUnselected?.Invoke(unit);
        }
    }
}
